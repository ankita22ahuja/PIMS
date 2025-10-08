
using PIMS.Model;
using System.Data.SqlClient;

namespace PIMS.Service
{
    public class User
    {
        private readonly string _conString;
        public User(IConfiguration config)
        {
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
    }
}
