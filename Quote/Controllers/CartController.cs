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

        [HttpGet("GetCartDetails/{cartId}")]
        public async Task<IActionResult> GetCartDetails(int cartId)
        {
            try
            {
                var cartDetails = await _cartService.GetCartsAsync();
                var cartDetailsOfCartId = cartDetails
                    .Where(cd => cd.CartId == cartId).ToList();

                var catrpList = new List<CartResponse>(); // Khởi tạo danh sách cart responses

                foreach (var cd in cartDetailsOfCartId)
                {
                    var pro = await _productService.GetProductAsync();
                    var prodt = pro.FirstOrDefault(p => p.ProductId == cd.ProductId);

                    var img = await _productService.GetImageAsync();
                    var imgdt = img.FirstOrDefault(p => p.ProductId == cd.ProductId);

                    var cartpush = new CartResponse()
                    {
                        CartDetailId = cd.CartDetailId,
                        CartId = cartId,
                        ProductId = (int)cd.ProductId,
                        ProductName = prodt?.ProductName, // Kiểm tra null trước khi truy cập
                        ProductThumbnail= imgdt?.Image1?.ToString() // Kiểm tra null trước khi truy cập
                    };

                    catrpList.Add(cartpush); // Thêm cartpush vào danh sách
                }

                return Ok(catrpList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult AddToCart(int userId, int productId)
        {
            try
            {
                var cart = _cartService.GetCartAsync().Result.SingleOrDefault(c => c.UserId == userId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                    };
                    _cartService.CreateCartOfUser(cart);
                }

                // Tạo chi tiết giỏ hàng
                var cartDetail = new CartDetail { CartId = cart.CartId, ProductId = productId };
                _cartService.AddCartAsync(cartDetail);

                return Ok("Sản phẩm đã được thêm vào giỏ hàng");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
            }
        }
        [HttpDelete("DeleteCartDetail/{cartDetailId}")]
        public async Task<IActionResult> DeleteCartDetail(int cartDetailId)
        {
            try
            {
                var cartDetail = await _cartService.GetCartsAsync();
                var cartDetails = await _cartService.GetCartsAsync();
                var cartDetailsOfCartId = cartDetails
                    .Where(cd => cd.CartDetailId == cartDetailId).FirstOrDefault();
                if (cartDetail == null)
                {
                    return NotFound($"Không tìm thấy CartDetail với ID {cartDetailId}");
                }

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
