using AutoMapper;
using Azure.Core;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal.request;
using Quote.Models;
using Quote.Repositorys;
using System.Diagnostics.Contracts;

namespace Quote.Services
{
    public class RequestService : IRequestService
    {
        private readonly RequestRepository _repo;
        private readonly ContractRepository _repoCt;

        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RequestService(RequestRepository repo, ContractRepository repoCt, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _repo = repo;
            _repoCt = repoCt;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Models.Contract> CreateContractUser(Models.Contract contract)
        {
            try
            {
                await _repoCt.AddAsync(contract);
                return contract;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }

        public  async Task<Models.Request> CreateRequestUser(Models.Request request)
        {
            try
            {

                var requestres = await _repo.AddReturnAsync(request);
                return requestres;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }

        public async Task<Models.Contract> UpdateContractUser(int contractId)
        {
            try
            {
                var contract = _repoCt.GetAllAsync().Result.Where(c => c.ContractId == contractId).FirstOrDefault();
                //contract.status = true;
                await _repoCt.UpdateAsync(contract);
                return contract;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }

        public Task<Models.Request> UpdateRequestUser(Models.Request request)
        {
            throw new NotImplementedException();
        }
    }
}
