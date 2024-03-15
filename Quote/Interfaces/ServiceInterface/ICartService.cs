using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface ICartService
    {
        Task<List<Cart>> GetCartsAsync();
        Task<Cart> AddCartAsync(Cart cartDetail);
        Task<Cart> GetCartIdAsync(int cartDetailId);
        Task<Boolean> DeleteCartDetail(int cartDetialID);

    }
}
