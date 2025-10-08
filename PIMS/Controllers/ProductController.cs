using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIMS.Model;
using PIMS.Service;

namespace PIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        //Insert 
        [HttpPost("CreateProduct")]

        public async Task<IActionResult> InsertPro([FromBody] ProductModel model)
        {
            try
            {
                string res = await _productService.InsertPro(model);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get All Pro
        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetProDetail()
        {
            try
            {
                ProductModel pro = await _productService.GetProduct();
                return Ok(pro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Update Product
        [HttpPost("UpPrice")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel model)
        {
            try
            {
                string uppro = await _productService.UpdateProPrice(model);
                return Ok(uppro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
