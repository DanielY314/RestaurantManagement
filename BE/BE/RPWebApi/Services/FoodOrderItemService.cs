using MySql.Data.MySqlClient;
using RPWebApi.Business;
using RPWebApi.Models;

namespace RPWebApi.Services
{
    public class FoodOrderItemService : BaseService
    {
        public FoodOrderItemService(IConfiguration configuration) : base(configuration) { }
        //  MySQL 을 사용해서 Category table 의 자료를 입력, 조회, 수정, 삭제

        public IEnumerable<FoodOrderItem> GetFoodOrderItems(int food_order_id) // all catetories
        {
            // db 연결, category table 조회, 조회한 결과를 Category List 에 대입, 리턴
            MySqlConnection con = null;

            List<FoodOrderItem> result = new List<FoodOrderItem>();

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "select * from food_order_item where food_order_id=@food_order_id";
                
                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@food_order_id", MySqlDbType.Int32).Value = food_order_id;

                // DB 접속해서  명령 수행후 메모리에 대입
                MySqlDataReader sdr = cm.ExecuteReader();

                while (sdr.Read()) // sdr 은 테이블 형식의 메모리 데이타 (즉 2차원 배열)
                {
                    result.Add(
                        new FoodOrderItem
                        {
                            id = sdr["id"] != DBNull.Value ? int.Parse(sdr["id"].ToString()) : -1,
                            food_id = sdr["food_id"] != DBNull.Value ? int.Parse(sdr["food_id"].ToString()) : -1,
                            food_order_id = sdr["food_order_id"] != DBNull.Value ? int.Parse(sdr["food_order_id"].ToString()) : -1,
                            count = sdr["count"] != DBNull.Value ? int.Parse(sdr["count"].ToString()) : 0
                        }
                    );
                }

                sdr.Close(); // 반드시 수동으로 닫아야 한다.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return result;
        }


        public bool AddFoodOrderItem(List<FoodCustomerOrder> fo)
        {
            // db 연결
            MySqlConnection con = null;

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // transaction 을 어떻게 구현할까? 즉 다음의 네가지 프로세스를 하나의 그룹안에서 실행하게끔 해서, 100% 실행, 100% 취소를 구현하는 것

                // DB 를 실제 연결
                con.Open();

                // 1. 빈 order 생성
                var sql = "insert into food_order (order_date,total) values(@order_date, 0)";
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@order_date", MySqlDbType.Datetime).Value = DateTime.Now;
                cm.ExecuteNonQuery();

                // 2. 생성되 order 를 바탕으로 item 을 입력
                sql = "select max(id) from food_order";
                MySqlCommand cm4 = new MySqlCommand(sql, con);
                var re = cm4.ExecuteScalar(); // sql 구문에 count, min, max, avg, sum 함수를 통해 값을 가져올 때
                int food_order_id = int.Parse(re.ToString());

                // food order item 에 입력
                decimal total = 0.00M;
                foreach(var member in fo)
                {
                    sql = "insert into food_order_item(food_id, food_order_id, count) values(@food_id, @food_order_id, @count)";
                    // 명령을 수행하는 object
                    MySqlCommand cm2 = new MySqlCommand(sql, con);
                    cm2.Parameters.Add("@food_id", MySqlDbType.Int32).Value = member.FoodType.id;
                    cm2.Parameters.Add("@food_order_id", MySqlDbType.Int32).Value = food_order_id;
                    cm2.Parameters.Add("@count", MySqlDbType.Int32).Value = member.Count;
                    // DB 접속해서  명령 수행
                    cm2.ExecuteNonQuery();

                    total += member.FoodType.sales_price * member.Count;
                }

                // 3.order 총 가격을 update / 실제 주문이 이뤄진 seat 을 입력
                sql = "update food_order set total=@total, seatName = @seatName where id=@id";
                MySqlCommand cm3 = new MySqlCommand(sql, con);
                cm3.Parameters.Add("@total", MySqlDbType.Decimal).Value = total;
                cm3.Parameters.Add("@seatName", MySqlDbType.VarChar).Value = fo.First().SeatName;
                cm3.Parameters.Add("@id", MySqlDbType.Int32).Value = food_order_id;
                cm3.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close();
            }

            return true;
        }

        public bool UpdateFoodOrderItem(int id, FoodOrder fo)
        {
            return false;
        }

        public bool DeleteFoodOrderItem(int id)
        {
            return true;
        }
    }
}
