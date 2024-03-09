using AutoMapper;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Repositorys;
using static System.Net.Mime.MediaTypeNames;

namespace Quote.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _repo;
        private readonly OptionRepository _repoOP;
        private readonly ImageRepository _repoIM;
        private readonly CategoryRepository _repoCategory;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

public ProductService(ProductRepository repo, OptionRepository repoOP, ImageRepository repoIM, CategoryRepository repoCategory, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _repo = repo;
            _repoOP = repoOP;
            _repoIM = repoIM;
            _repoCategory = repoCategory;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        private string GetFilepath(string code)
        {
            return this._webHostEnvironment.WebRootPath + "\\Upload\\product\\" + code;
        }

        public async Task<int> AddProduct(Product product)
        {       
            try
            {
                
                await _repo.AddAsync(product);
                return product.ProductId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }

        }

        

        //public async Task<List<Image>> GetImageAsync()
        //{
        //    try
        //    {
        //        return await _repoIM.GetAllAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error getting users: " + ex.Message);
        //    }
        //}

        public async Task<List<Option>> GetOptionAsync()
        {
            try
            {
                return await _repoOP.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }

        public async Task<List<Product>> GetProductAsync()
        {
            try
            {
                return await _repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }

        public async Task<List<Models.Image>> GetImageAsync()
        {
            try
            {
                return await _repoIM.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }

        public async Task<Product> DeleteProduct(int id)
        {
            try
            {
               var product =  await _repo.GetByIdAsync(id);
                if (product != null)
                {
                    product.IsDelete = true;
                    var pro =  await _repo.UpdateAsync(product);
                    return pro;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }

        public async Task<Product> UpdateProduct(int id, ProductModal product)
        {
            try
            {
                var pro = await _repo.GetByIdAsync(id);
                if (pro != null)
                {
                    pro.Price = product.Price;
                    pro.ProductName = product.ProductName;
                    pro.CategoryId = product.CategoryId;
                    pro.Description = product.Description;
                    var newpro = await _repo.UpdateAsync(pro);         
                    return newpro;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }
        public async Task<List<Category>> GetCategoryAsync()
        {

            try
            {
                return await _repoCategory.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }
        public async System.Threading.Tasks.Task Save()
        {
            await _repo.Save();
        }
    }
}
