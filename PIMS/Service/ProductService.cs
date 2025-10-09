using PIMS.Model;
using System.Data.SqlClient;

namespace PIMS.Service
{

    public class ProductService
    {
        private readonly string _conString;
        public ProductService(IConfiguration config)
        {
            _conString = config.GetConnectionString("SqlConnection");
        }

        //Get All Product Detail
        public async Task<ProductModel> GetProduct()
        {
            ProductModel model = new ProductModel();

            using (SqlConnection con = new SqlConnection(_conString))
            {
                await con.OpenAsync();
                string query = "select Pro.SKU,Pro.Name,Pro.Description,Pro.Price,pro.CategoryId,cat.CategoryName from Products Pro,Categories cat where pro.CategoryId=cat.CategoryId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Clear();
                    //cmd.Parameters.AddWithValue("user", model); 
                    using (SqlDataReader red = cmd.ExecuteReader())
                    {
                        while (red.Read())
                        {
                            model = new ProductModel
                            {
                                SKU = red["SKU"].ToString(),
                                Name = red["Name"].ToString(),
                                Desciption = red["Description"].ToString(),
                                Price = (decimal)red["Price"],
                                CatId = (int)red["CategoryId"]

                            };
                        }
                    }
                }

                return model;

            }
        }

        //Insert Product in Table

        public async Task<string> InsertPro(ProductModel model)
        {

            if (model.Price <= 0)
            {
                return "Price must be greater than zero";
            }
            string res = "";
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                //Check Unique
                string checkquery = "select count(SKU) from products where SKU=@SKU";
                using (SqlCommand cmd = new SqlCommand(checkquery, con))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@sku", model.SKU);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        return "SKU already exist";
                    }

                }

                //Insert Product
                string insertQuery = @"INSERT INTO Products (SKU, Name, Description, Price, CategoryId, CreatedDate)
                               VALUES (@SKU, @Name, @Description, @Price, @CategoryId, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@SKU", model.SKU);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Description", model.Desciption);
                    cmd.Parameters.AddWithValue("@Price", model.Price);
                    cmd.Parameters.AddWithValue("@CategoryId", model.CatId);

                    int affectedRows = await cmd.ExecuteNonQueryAsync();
                    res = affectedRows.ToString();

                }
                return res;
            }
        }

        //UPdate Product Price
  
        public async Task<string> UpdateProPrice(decimal Price,string SKU)
        {
            if (Price <= 0)
            {
                return "Price must be greater than zero";
            }
            string res = "";
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();

                //Up Product
                string UpdateQuery = "UPDATE Products " +
                              " SET  Price = @Price" +
                              " WHERE SKU = @SKU ";

                using (SqlCommand cmd = new SqlCommand(UpdateQuery, con))
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@Price", Price);
                    cmd.Parameters.AddWithValue("@SKU", SKU);

                    int affectedRows = await cmd.ExecuteNonQueryAsync();
                    res = affectedRows.ToString();

                }
                return res;
            }
        }
    }
}
