using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Services;

namespace Quote.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PoductController : ControllerBase
    {
        private readonly IProductService _productService;


        public PoductController(IProductService productServic)
        {
            _productService = productServic;
        }
        public class ProductRequestModel
        {
            public int Idproduct { get; set; }
        }
        [HttpPost("GetProduct")]
        public IActionResult GetProduct([FromBody] ProductRequestModel requestModel)
        {
            try
            {
                var product = _productService.GetProductAsync().Result.SingleOrDefault(p => p.ProductId == requestModel.Idproduct);
                var Imgproduct = _productService.GetImageAsync().Result.Where(i => i.ProductId == product.ProductId).ToList();
                var options = _productService.GetOptionAsync().Result.Where(o => o.ProductId == product.ProductId).ToList();

                if (product == null)
                {
                    return NotFound();
                }
                var productWithOptions = new ProductResponse
                {
                    Product = product,
                    Options = options
                };

                return Ok(productWithOptions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }

        }
       
    }
}
