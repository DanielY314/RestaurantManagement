using MySql.Data.MySqlClient;
using RPWebApi.Models;

namespace RPWebApi.Services
{
    public class FoodOrderService : BaseService
    {
        public FoodOrderService(IConfiguration configuration) : base(configuration) { }
        //  MySQL 을 사용해서 Category table 의 자료를 입력, 조회, 수정, 삭제

        public IEnumerable<FoodOrder> GetFoodOrders() // all catetories
        {
            var result = GetAllFoodOrders();
            return result;
        }

        public IEnumerable<FoodOrder> GetFoodOrders(DateTime startDate, DateTime endDate) // all catetories
        {
            var result = GetAllFoodOrders();
            var dateResult = result.Where(x => x.order_date >= startDate && x.order_date <= endDate).ToList();
            return dateResult;
        }

        public FoodOrder GetFoodOrder(int id) // one foodOrder
        {
            var result = GetAllFoodOrders();
            var dateResult = result.Where(x => x.id== id).FirstOrDefault();
            return dateResult;
        }

        private IEnumerable<FoodOrder> GetAllFoodOrders()
        {
            // db 연결, category table 조회, 조회한 결과를 Category List 에 대입, 리턴
            MySqlConnection con = null;

            List<FoodOrder> result = new List<FoodOrder>();

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "select * from food_order";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);


                // DB 접속해서  명령 수행후 메모리에 대입
                MySqlDataReader sdr = cm.ExecuteReader();

                while (sdr.Read()) // sdr 은 테이블 형식의 메모리 데이타 (즉 2차원 배열)
                {
                    result.Add(
                        new FoodOrder
                        {
                            id = sdr["id"] != DBNull.Value ? int.Parse(sdr["id"].ToString()) : -1,
                            order_date = sdr["order_date"] != DBNull.Value ? DateTime.Parse(sdr["order_date"].ToString()) : DateTime.MinValue,
                            total = sdr["total"] != DBNull.Value ? Decimal.Parse(sdr["total"].ToString()) : 0.0M,
                            seatName = sdr["seatName"] != DBNull.Value ? sdr["seatName"].ToString() : string.Empty
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

        public bool AddFoodOrder(FoodOrder fo)
        {
            // db 연결
            MySqlConnection con = null;

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "insert into food_order(order_date, total) values(@order_date, @total)";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@order_date", MySqlDbType.Datetime).Value = fo.order_date;
                cm.Parameters.Add("@total", MySqlDbType.Decimal).Value = fo.total;

                // DB 접속해서  명령 수행
                int i = cm.ExecuteNonQuery();
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

        public bool UpdateFoodOrder(int id, FoodOrder fo)
        {
            // db 연결
            MySqlConnection con = null;

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                // check if id exist
                var sql = "select count(*) from food_order where id=@id";
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                var re = cm.ExecuteScalar(); // sql 구문에 count, min, max, avg, sum 함수를 통해 값을 가져올 때

                if (int.Parse(re.ToString()) != 1)
                    return false;

                sql = "update food_order set order_date=@order_date, total=@total where id=@id";

                // 명령을 수행하는 object
                MySqlCommand cm2 = new MySqlCommand(sql, con);
                cm2.Parameters.Add("@order_date", MySqlDbType.Datetime).Value = fo.order_date;
                cm2.Parameters.Add("@total", MySqlDbType.Decimal).Value = fo.total;
                cm2.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                // DB 접속해서  명령 수행
                int i = cm2.ExecuteNonQuery();
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

        public bool DeleteFoodOrder(int id)
        {
            // db 연결
            MySqlConnection con = null;

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "delete from food_order where id=@id";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                

                // DB 접속해서  명령 수행
                int i = cm.ExecuteNonQuery();
                if(i < 1)
                {
                    return false;
                }
                return true;
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
    }
}
