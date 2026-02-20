using MySql.Data.MySqlClient;
using RPWebApi.Models;

namespace RPWebApi.Services
{
    public class foodService: BaseService
    {
        public foodService(IConfiguration configuration) : base(configuration) { }

        //  MySQL 을 사용해서 Category table 의 자료를 입력, 조회, 수정, 삭제

        public IEnumerable<Food> GetFoods() // all products
        {
            var result = GetAllFoods();
            return result;
        }

        public IEnumerable<Food> GetFoods(int categoryId) // all products
        {
            var result = GetAllFoods();
            var food = result.Where(x => x.category_id == categoryId).ToList();
            return food;
        }

        public Food GetFood(int id) // one product
        {
            var result = GetAllFoods();
            var food = result.Where(x => x.id == id).FirstOrDefault();
            return food;
        }

        public Food GetFood(string name) // one product
        {
            var result = GetAllFoods();
            var food = result.Where(x => x.name.ToLower() == name.ToLower()).FirstOrDefault();
            return food;
        }

        private List<Food> GetAllFoods()
        {
            // db 연결, category table 조회, 조회한 결과를 Category List 에 대입, 리턴
            MySqlConnection con = null;

            List<Food> result = new List<Food>();

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "select * from food";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);


                // DB 접속해서  명령 수행후 메모리에 대입
                MySqlDataReader sdr = cm.ExecuteReader();

                while (sdr.Read()) // sdr 은 테이블 형식의 메모리 데이타 (즉 2차원 배열)
                {
                    result.Add(
                        new Food
                        {
                            id = sdr["id"] != DBNull.Value ? int.Parse(sdr["id"].ToString()) : -1,
                            category_id = sdr["category_id"] != DBNull.Value ? int.Parse(sdr["category_id"].ToString()) : -1,
                            name = sdr["name"] != DBNull.Value ? sdr["name"].ToString() : "",
                            original_price = sdr["original_price"] != DBNull.Value ? decimal.Parse(sdr["original_price"].ToString()) : 0.0M,
                            sales_price = sdr["sales_price"] != DBNull.Value ? decimal.Parse(sdr["sales_price"].ToString()) : 0.0M,
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
        public bool AddFood(Food fo)
        {
            // db 연결
            MySqlConnection con = null;

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "insert into food(category_id, name, original_price,sales_price) values(@category_id, @name, @original_price, @sales_price)";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@category_id", MySqlDbType.Int32).Value = fo.category_id;
                cm.Parameters.Add("@name", MySqlDbType.String).Value = fo.name;
                cm.Parameters.Add("@original_price", MySqlDbType.Decimal).Value = fo.original_price;
                cm.Parameters.Add("@sales_price", MySqlDbType.Decimal).Value = fo.sales_price;

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

        public bool UpdateFood(int id, Food fo)
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
                var sql = "select count(*) from food where id=@id";
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                var re = cm.ExecuteScalar(); // sql 구문에 count, min, max, avg, sum 함수를 통해 값을 가져올 때

                if (int.Parse(re.ToString()) != 1)
                    return false;

                sql = "update food set category_id=@category_id, name=@name, original_price=@original_price, sales_price=@sales_price where id=@id";

                // 명령을 수행하는 object
                MySqlCommand cm2 = new MySqlCommand(sql, con);
                cm2.Parameters.Add("@category_id", MySqlDbType.Int32).Value = fo.category_id;
                cm2.Parameters.Add("@name", MySqlDbType.String).Value = fo.name;
                cm2.Parameters.Add("@original_price", MySqlDbType.Decimal).Value = fo.original_price;
                cm2.Parameters.Add("@sales_price", MySqlDbType.Decimal).Value = fo.sales_price;

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

        public bool DeleteFood(int id)
        {
            // db 연결
            MySqlConnection con = null;

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "delete from food where id=@id";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;


                // DB 접속해서  명령 수행
                int i = cm.ExecuteNonQuery();
                if (i < 1)
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
