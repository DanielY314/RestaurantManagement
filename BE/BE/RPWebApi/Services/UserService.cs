using MySql.Data.MySqlClient;
using RPWebApi.Dto;
using RPWebApi.Models;

namespace RPWebApi.Services
{
    public class userService : BaseService
    {
        public IEnumerable<CategoryDTO> GetCategories() // all catetories
        {
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

        public bool AddUser(User u, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            //u.passwordHash = passwordHash;
            //u.passwordSalt = passwordSalt;

            // db 연결
            MySqlConnection con = null;

            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "insert into user(username, passwordHash, passwordSalt, createDate) values(@username, @passwordHash, @passwordSalt, @createDate)";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);

                cm.Parameters.Add("@username", MySqlDbType.String).Value = u.Username;
                cm.Parameters.Add("@passwordHash", MySqlDbType.Blob).Value = passwordHash;
                cm.Parameters.Add("@passwordSalt", MySqlDbType.Blob).Value = passwordSalt;
                cm.Parameters.Add("@createDate", MySqlDbType.DateTime).Value = DateTime.Now;

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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public bool UserExist(string username)
        {
            // db 연결, category table 조회, 조회한 결과를 Category List 에 대입, 리턴
            MySqlConnection con = null;


            try
            {
                // DB 연결 object
                con = new MySqlConnection(connectionString);

                // DB 를 실제 연결
                con.Open();

                var sql = "select count(*) from user where username = '" + username.Trim() + "'";

                // 명령을 수행하는 object
                MySqlCommand cm = new MySqlCommand(sql, con);


                var count  = cm.ExecuteScalar(); // min, max, count, avg, sum
                int iExist = int.Parse(count.ToString());
                
                return iExist > 0;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
