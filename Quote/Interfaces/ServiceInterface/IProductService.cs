using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IProductService
    {
        Task<List<Product>> GetProductAsync();
        Task<List<Image>> GetImageAsync();
        Task<List<Option>> GetOptionAsync();
        Task<List<Category>> GetCategoryAsync();
        Task<Category> GetCategoryIdAsync(int cartId);
        System.Threading.Tasks.Task DeleteProduct(int id);

        System.Threading.Tasks.Task Save();

        Task<Product> UpdateProduct(int id,ProductModal product);

        Task<int> AddProduct(Product product);

        Task<List<Product>> GetAllProduct();

        Task<Product> GetProductId(int id);

    }
}
