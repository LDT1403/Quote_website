using AutoMapper;
using Quote.Interfaces.RepositoryInterface;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Repositorys;
using static System.Net.Mime.MediaTypeNames;

namespace Quote.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepoBase<Models.Product> _repo;
        private readonly IRepoBase<Models.Option> _repoOP;
        private readonly IRepoBase<Models.Image> _repoIM;
        private readonly IRepoBase<Category> _repoCategory;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

public ProductService(IRepoBase<Models.Product> repo, IRepoBase<Models.Option> repoOP, IRepoBase<Models.Image> repoIM, IRepoBase<Category> repoCategory, IMapper mapper, IWebHostEnvironment webHostEnvironment)
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

        public async Task<List<Product>> GetAllProduct()
        {
            var listPro = await _repo.GetAllAsync();
            if(listPro != null)
            {
                listPro = listPro.Where(p => p.IsDelete != true).ToList();
                return listPro;
            }
            return null;
            
        }

        public async Task<Product> GetProductId(int id)
        {
            var pro = await _repo.GetByIdAsync(id);

             if(pro != null)
            {
                return pro;
            }
            return null;
        }

        public async Task<Category> GetCategoryIdAsync(int cartId)
        {
            var cate = await _repoCategory.GetByIdAsync(cartId);

            if (cate != null)
            {
                return cate;
            }
            return null;
        }
    }
}
