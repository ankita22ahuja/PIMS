using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using PIMS.Service;
using PIMS.Model;

namespace PIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _conString;
        private readonly User _userService;

        public UsersController(IConfiguration config, User userService)
        {
            _conString = config.GetConnectionString("SqlConnection");
            _userService = userService;
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

        [HttpGet("GetUserAuth")]
        //Get All User
        public async Task<IActionResult> GetUserDetail()
        {
            UserModel user = await _userService.GetUser();
            return Ok(user);
        }


    }
}
