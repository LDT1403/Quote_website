using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IImageService
    {
        System.Threading.Tasks.Task AddImage(Image img);

        Task<Image> DeleteImg(int id);

        Task<List<Image>> GetImgById(int productId);
    }
}
