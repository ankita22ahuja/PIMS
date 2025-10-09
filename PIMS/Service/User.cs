
using Microsoft.IdentityModel.Tokens;
using PIMS.Model;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PIMS.Service
{
    public class User
    {
        private readonly string _conString;
        private readonly IConfiguration _config;
        public User(IConfiguration config)
        {
            _config = config;
            _conString = config.GetConnectionString("SqlConnection");
        }

        //Get All User Detail
        public async Task<UserModel> GetUser()
        {
            UserModel model = new UserModel();

            using (SqlConnection con = new SqlConnection(_conString))
            {
                await con.OpenAsync();
                string query = "select UserId from Users";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Clear();
                    //cmd.Parameters.AddWithValue("user", model); 
                    using (SqlDataReader red = cmd.ExecuteReader())
                    {
                        while (red.Read())
                        {
                            model = new UserModel
                            {
                                UserId = (int)red["UserId"],
                            };
                        }
                    }
                }

                return model;

            }
        }

        //Generate JWT token by UserID
        public async Task<string> GenerateTokenByUserId(int UserId)
        {
            UserModel model = new UserModel();

            using (SqlConnection con = new SqlConnection(_conString))
            {
                await con.OpenAsync();
                string query = "select RoleId,UserName from Users where UserId=@UserId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    using (SqlDataReader red = cmd.ExecuteReader())
                    {
                        while (red.Read())
                        {
                            model = new UserModel
                            {
                                RoleId = (int)red["RoleId"],
                                UserName = (string)red["UserName"]
                            };
                        }
                        var roleName = "";
                        if (model.RoleId == 1)
                        {
                            roleName = "Admin";
                        }
                        else
                        {
                            roleName = "User";
                        }

                        //Genearte JWT
                        var jwtSection = _config.GetSection("Jwt");
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.GetValue<string>("Key")));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, model.UserName),
                            new Claim(ClaimTypes.Role, roleName),
                            new Claim("UserID", model.UserId.ToString())
                        };

                        var token = new JwtSecurityToken(
                        issuer: jwtSection.GetValue<string>("Issuer"),
                        audience: jwtSection.GetValue<string>("Audience"),
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(6),
                        signingCredentials: creds
                        );


                        return new JwtSecurityTokenHandler().WriteToken(token);
                    }

                }
            }
        }
    }
}

