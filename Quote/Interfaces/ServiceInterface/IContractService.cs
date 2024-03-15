using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IContractService
    {
        Task<List<Contract>> GetAllContractsAsync();

        Task<List<Contract>> GetNewContract();
    }
}
