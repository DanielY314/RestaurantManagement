using MySql.Data.MySqlClient;
using RPWebApi.Dto;
using RPWebApi.Models;

namespace RPWebApi.Services
{
    public class categoryService : BaseService
    {
        public categoryService(IConfiguration configuration) : base(configuration) { }

        //  MySQL 을 사용해서 Category table 의 자료를 입력, 조회, 수정, 삭제

        public IEnumerable<CategoryDTO> GetCategories() // all catetories
        {
            // db 연결, category table 조회, 조회한 결과를 Category List 에 대입, 리턴
            MySqlConnection con = null;

            List<CategoryDTO> result = new List<CategoryDTO>();

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);
                
                // DB 를 실제 연결
                con.Open();

                var sql = "select * from category";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);
                

                // DB 접속해서  명령 수행후 메모리에 대입
                MySqlDataReader sdr = cm.ExecuteReader();

                while (sdr.Read()) // sdr 은 테이블 형식의 메모리 데이타 (즉 2차원 배열)
                {
                    result.Add(
                        new CategoryDTO
                        {
                             categoryId = sdr["id"] != DBNull.Value ? int.Parse(sdr["id"].ToString()) : -1,
                             name = sdr["name"] != DBNull.Value ? sdr["name"].ToString() : "",
                             description = sdr["description"] != DBNull.Value ? sdr["description"].ToString() : ""
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

        public bool AddCategory(Category fo)
        {
            // db 연결
            MySqlConnection con = null;

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "insert into category(name, description) values(@name, @description)";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);
                
                cm.Parameters.Add("@name", MySqlDbType.String).Value = fo.name;
                cm.Parameters.Add("@description", MySqlDbType.String).Value = fo.description;

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

        public bool UpdateCategory(int id, Category fo)
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
                var sql = "select count(*) from category where id=@id";
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                var re = cm.ExecuteScalar(); // sql 구문에 count, min, max, avg, sum 함수를 통해 값을 가져올 때

                if (int.Parse(re.ToString()) != 1)
                    return false;

                sql = "update category set name=@name, description=@description where id=@id";

                // 명령을 수행하는 object
                MySqlCommand cm2 = new MySqlCommand(sql, con);
                cm2.Parameters.Add("@name", MySqlDbType.String).Value = fo.name;
                cm2.Parameters.Add("@description", MySqlDbType.String).Value = fo.description;

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

        //public bool DeleteCategory(int id)
        //{
        //    // db 연결
        //    MySqlConnection con = null;

        //    try
        //    {
        //        // DB 연결 object
        //        con = new MySqlConnection(connectionString);

        //        // DB 를 실제 연결
        //        con.Open();

        //        var sql = "delete from food where id=@id";

        //        // 명령을 수행하는 object
        //        MySqlCommand cm = new MySqlCommand(sql, con);
        //        cm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;


        //        // DB 접속해서  명령 수행
        //        int i = cm.ExecuteNonQuery();
        //        if (i < 1)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }

        //    return true;
        //}
    }
}
