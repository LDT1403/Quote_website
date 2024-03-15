using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Services;

namespace Quote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        [HttpGet("GetCartDetails")]
        public async Task<IActionResult> GetCartDetails(int userId)
        {
            try
            {
                var cartDetails = _cartService.GetCartsAsync().Result.Where(c => c.UserId == userId).ToList();
                if (cartDetails == null)
                {
                    return null;
                }
                var catrpList = new List<CartResponse>();
                foreach (var cd in cartDetails)
                {
                    var pro = await _productService.GetProductAsync();

                    var prodt = pro.FirstOrDefault(p => p.ProductId == cd.ProductId);
                    if (prodt?.IsDelete != true)
                    {
                        var img = await _productService.GetImageAsync();
                        var imgdt = img.FirstOrDefault(p => p.ProductId == cd.ProductId);
                        var cartpush = new CartResponse()
                        {
                            CartDetailId = cd.CartDetailId,
                            //CartId = cd.CartDetailId,
                            ProductId = (int)cd.ProductId,
                            ProductName = prodt?.ProductName,
                            ProductThumbnail = imgdt?.ImagePath?.ToString()
                        };
                        catrpList.Add(cartpush);
                    }
                }

                return Ok(catrpList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
            }
        }
        [HttpGet("LoadAdd")]
        public async Task<IActionResult> LoadAdd()
        {
            return Ok();
        }
        [HttpPost("AddToCart")]
        public IActionResult AddToCart(int userId, int productId)
        {
            try
            {

                var checkPro = _cartService.GetCartsAsync().Result.Where(c => c.UserId == userId).ToList();
                foreach (var p in checkPro)
                {
                    if (p.ProductId == productId)
                    {
                        return Ok("Bạn đã thêm sản phẩm này rồi !");
                    }
                }
                var cartDetail = new Cart { UserId = userId, ProductId = productId };
                _cartService.AddCartAsync(cartDetail);

                return Ok("Sản phẩm đã được thêm vào Yêu Thích");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
            }
        }
        [HttpDelete("DeleteCartDetail")]
        public async Task<IActionResult> DeleteCartDetail(int cartDetailId)
        {
            try
            {

                var cartDetails = await _cartService.GetCartIdAsync(cartDetailId);
                await _cartService.DeleteCartDetail(cartDetailId);

                return Ok("CartDetail đã được xóa thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
            }
        }
    }
}
