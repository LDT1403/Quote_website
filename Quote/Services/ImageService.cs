using AutoMapper;
using Quote.Interfaces.ServiceInterface;
using Quote.Models;
using Quote.Repositorys;

namespace Quote.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageRepository _repo;
        private readonly IMapper _mapper;

       public ImageService(ImageRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async System.Threading.Tasks.Task AddImage(Image img)
        {
            await _repo.AddAsync(img);
        }

        public async Task<Image> DeleteImg(int id)
        {
           var pro = _repo.GetAllAsync().Result.Where(p=>p.ProductId == id).FirstOrDefault();

            if (pro != null)
            {
                var img = await _repo.DeleteAsync(pro);
                return img;
            }
            return null;
        }

      

        public async Task<List<Image>> GetImgById(int productId)
        {
            var list = await _repo.GetAllAsync();
            if(list != null)
            {
                list =list.Where(p => p.ProductId == productId).ToList();
                return list;
            }
            return null;
        }
    }
}
