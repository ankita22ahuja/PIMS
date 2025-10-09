using PIMS.Model;
using System.Data.SqlClient;

namespace PIMS.Service
{
    public class InventoryService
    {
        private readonly string _conString;
        public InventoryService(IConfiguration config)
        {
            _conString = config.GetConnectionString("SqlConnection");
        }

        //Get All invetory Detail
        public async Task<InventoryModel> GetInventory()
        {
            InventoryModel model = new InventoryModel();

            using (SqlConnection con = new SqlConnection(_conString))
            {
                await con.OpenAsync();
                string query = "SELECT pro.name,inv.ProductID, inv.Quantity, inv.Threshold FROM Inventory inv,Products pro\r\nWHERE inv.productid=pro.productid and inv.Quantity < inv.Threshold";
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        model = (new InventoryModel
                        {
                            ProductName = rdr["name"].ToString(),
                            Quanity = (int)rdr["Quantity"],
                            Threshold = (int)rdr["Threshold"]
                        });
                    }
                }
            }

            return model;
        }

    }
}

