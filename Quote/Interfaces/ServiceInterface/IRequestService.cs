using Quote.Modal.request;
using Quote.Models;
using Quote.Models;
namespace Quote.Interfaces.ServiceInterface
{
    public interface IRequestService
    {
        Task<Request> UpdateRequestUser(Request request);
        Task<Request> CreateRequestUser(Request request);
        Task<Contract> CreateContractUser(Contract contract);
        Task<Contract> UpdateContractUser(Contract contract);
    }

}
