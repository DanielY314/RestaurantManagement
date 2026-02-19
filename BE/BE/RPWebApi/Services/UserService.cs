using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using RPWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RPWebApi.Services
{
    public class userService : BaseService
    {
        private readonly IConfiguration _configuration;

        public userService(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public bool AddUser(User u, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            MySqlConnection con = null;
            try
            {
                con = new MySqlConnection(connectionString);
                con.Open();
                var sql = "insert into user(username, passwordHash, passwordSalt, createDate) values(@username, @passwordHash, @passwordSalt, @createDate)";
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@username", MySqlDbType.String).Value = u.Username;
                cm.Parameters.Add("@passwordHash", MySqlDbType.Blob).Value = passwordHash;
                cm.Parameters.Add("@passwordSalt", MySqlDbType.Blob).Value = passwordSalt;
                cm.Parameters.Add("@createDate", MySqlDbType.DateTime).Value = DateTime.Now;
                cm.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
            return true;
        }

        public User GetUser(string username)
        {
            MySqlConnection con = null;
            try
            {
                con = new MySqlConnection(connectionString);
                con.Open();
                var sql = "select * from user where username = @username";
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@username", MySqlDbType.String).Value = username;
                MySqlDataReader sdr = cm.ExecuteReader();
                if (sdr.Read())
                {
                    var user = new User
                    {
                        Id = sdr["id"] != DBNull.Value ? int.Parse(sdr["id"].ToString()) : -1,
                        Username = sdr["username"].ToString(),
                        passwordHash = (byte[])sdr["passwordHash"],
                        passwordSalt = (byte[])sdr["passwordSalt"],
                    };
                    sdr.Close();
                    return user;
                }
                sdr.Close();
                return null;
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

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public string GenerateJwtToken(string username)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiryHours = int.Parse(_configuration["JwtSettings:ExpiryInHours"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(expiryHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool UserExist(string username)
        {
            MySqlConnection con = null;
            try
            {
                con = new MySqlConnection(connectionString);
                con.Open();
                var sql = "select count(*) from user where username = @username";
                MySqlCommand cm = new MySqlCommand(sql, con);
                cm.Parameters.Add("@username", MySqlDbType.String).Value = username.Trim();
                var count = cm.ExecuteScalar();
                return int.Parse(count.ToString()) > 0;
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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}