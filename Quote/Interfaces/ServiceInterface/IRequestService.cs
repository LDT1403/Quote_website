using Quote.Modal.request;
using Quote.Models;
using Quote.Models;
namespace Quote.Interfaces.ServiceInterface
{
    public interface IRequestService
    {
        Task<Request> UpdateRequestUser(Request request);
        Task<Request> GetRequestById(int requestId);
        Task<List<Request>> GetAllRequest();
        Task<List<Request>> GetRequestOfStatus();
        Task<Request> CreateRequestUser(Request request);
        Task<Contract> CreateContractUser(Contract contract);
        Task<List<Contract>> GetContract();
        Task<Request> Appoinment(int requestid);
        Task<Contract> UpdateContractUser(Contract contract);
        Task<Contract> UpdateContractUserId(int contractId);
    }

}
