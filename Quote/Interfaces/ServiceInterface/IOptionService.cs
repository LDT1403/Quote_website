using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IOptionService 
    {
        System.Threading.Tasks.Task AddOptions(Option opt);

        Task<Option> Update(int productId, Options[] option);

        Task<List<Option>> GetOptionById(int productId);

    }
}
