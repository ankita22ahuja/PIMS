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
            string res = await _productService.InsertPro(model);
            return Ok(res);
        }

        //Get All User
        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetUserDetail()
        {
            ProductModel pro = await _productService.GetProduct();
            return Ok(pro);
        }

        //Update Product
        [HttpPost("UpPrice")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel model)
        {
            string uppro = await _productService.UpdateProPrice(model);
            return Ok(uppro);
        }
    }
}
