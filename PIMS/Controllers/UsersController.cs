using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace PIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _conString;

        public UsersController(IConfiguration config)
        {
            _conString = config.GetConnectionString("SqlConnection");
        }

        [HttpGet("TestConn")]
        public IActionResult GetCon()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                }
                return Ok("Succesfully Connection Done");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get All User
        public IActionResult GetUserDetail()
        {
            return Ok();
        }
    }
}
