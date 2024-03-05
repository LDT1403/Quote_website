using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface ICartService
    {
        Task<List<CartDetail>> GetCartsAsync();
        Task<CartDetail> AddCartAsync(CartDetail cartDetail);
        Task<List<Cart>> GetCartAsync();
        Task<Cart> CreateCartOfUser(Cart cart);

        Task<Boolean> DeleteCartDetail(int cartDetialID);

    }
}
