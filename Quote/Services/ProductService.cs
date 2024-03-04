using AutoMapper;
using Quote.Interfaces.ServiceInterface;
using Quote.Models;
using Quote.Repositorys;

namespace Quote.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _repo;
        private readonly OptionRepository _repoOP;
        private readonly ImageRepository _repoIM;
        private readonly IMapper _mapper;

        public ProductService(ProductRepository productRepository, OptionRepository optionRepository, ImageRepository imageRepository, IMapper mapper)
        {
            _repo = productRepository;
            _repoOP = optionRepository;
            _repoIM = imageRepository;
            _mapper = mapper;
        }

        public async Task<List<Image>> GetImageAsync()
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
    }
}
