using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
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
        [HttpGet("GetProduct")]
        public IActionResult GetProduct([FromQuery] int Idproduct)
        {
            try
            {
                var product = _productService.GetProductAsync().Result.SingleOrDefault(p => p.ProductId == Idproduct);
                var category = _productService.GetCategoryAsync().Result.SingleOrDefault(c => c.CategoryId == product.CategoryId);
                var Imgproduct = _productService.GetImageAsync().Result.Where(i => i.ProductId == product.ProductId).ToList();
                var options = _productService.GetOptionAsync().Result.Where(o => o.ProductId == product.ProductId).ToList();

                if (product == null)
                {
                    return NotFound();
                }

                var productWithOptions = new ProductResponse
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    CateName = category.CateName,
                    Options = options,
                    Images = Imgproduct,
                };

                return Ok(productWithOptions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }

        }
        [HttpGet("GetProductCategory")]
        public IActionResult GetProductCategory([FromQuery] int Idcategory)
        {
            try
            {
                var product = new List<Product>();
                if (Idcategory == 10)
                {
                    product = _productService.GetProductAsync().Result.Where(p => (p.CategoryId == 1 && p.CategoryId  == 2 ) && p.IsDelete == false).ToList();
                }else
                if (Idcategory == 11)
                {
                    product = _productService.GetProductAsync().Result.Where(p => (p.CategoryId == 3 && p.CategoryId == 4 && p.CategoryId==5) && p.IsDelete == false).ToList();
                }
                else
                if (Idcategory != 11 && Idcategory != 10)
                {
                    product = _productService.GetProductAsync().Result.Where(p => p.CategoryId == Idcategory  && p.IsDelete == false).ToList();
                }             


                if (product == null)
                {
                    return NotFound();
                }

                var productWithCate = new List<ProductCateResponse>();
                foreach (var cate in product)
                {
                    var img = _productService.GetImageAsync().Result.Where(i => i.ProductId == cate.ProductId).FirstOrDefault();
                    var productCate = new ProductCateResponse
                    {
                        ProductId = cate.ProductId,
                        ImagePath = img?.ImagePath,
                    };
                    productWithCate.Add(productCate);
                }



                return Ok(productWithCate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }

        }


    }
}
