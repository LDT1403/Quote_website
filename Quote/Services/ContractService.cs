using Quote.Interfaces.RepositoryInterface;
using Quote.Interfaces.ServiceInterface;
using Quote.Models;
using Quote.Repositorys;

namespace Quote.Services
{
    public class ContractService : IContractService
    {
        private readonly IRepoBase<Contract> _repo;

        public ContractService(IRepoBase<Contract> repo)
        {
            _repo = repo;
        }
        public async Task<List<Contract>> GetAllContractsAsync()
        {
            try
            {
                var list = await _repo.GetAllAsync();
                return list;

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
