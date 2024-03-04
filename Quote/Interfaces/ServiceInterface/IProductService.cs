using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IProductService
    {
        Task<List<Product>> GetProductAsync();
        Task<List<Image>> GetImageAsync();
        Task<List<Option>> GetOptionAsync();

    }
}
