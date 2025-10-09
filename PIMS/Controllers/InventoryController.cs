using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIMS.Model;
using PIMS.Service;

namespace PIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _invetoryService;

        public InventoryController(InventoryService invetoryService)
        {
            _invetoryService = invetoryService;
        }

        //Get All Threshold  Inventory
        [HttpGet("GetProductThreshold")]
        public async Task<IActionResult> GetInDetail()
        {
            try
            {
                InventoryModel model = await _invetoryService.GetInventory();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
