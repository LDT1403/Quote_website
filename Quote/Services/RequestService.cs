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

        public async Task<Models.Request> Appoinment(int requestid)
        {
            try
            {
                var req = await _repo.GetByIdAsync(requestid);
                if (req != null)
                {
                    req.Status = "2";
                    var pro = await _repo.UpdateAsync(req);
                    return pro;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }

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

        public async Task<Models.Request> CreateRequestUser(Models.Request request)
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

        public async Task<List<Models.Request>> GetAllRequest()
        {
            var list = await _repo.GetAllAsync();
            if(list != null)
            {
                
                return list;
            }
            return null;
        }


        public async Task<List<Models.Contract>> GetContract()
        {
            var list = await _repoCt.GetAllAsync();
            if (list != null)
            {

                return list;
            }
            return null;
        }

        public async Task<Models.Request> GetRequestById(int requestId)
        {
            try {
                var request = await _repo.GetByIdAsync(requestId);
                return request;
            }
            catch (Exception ex) { 
                
                throw new Exception("Error getting users: " + ex.Message); }
        }

        public Task<List<Models.Request>> GetRequestOfStatus()
        {
            throw new NotImplementedException();
        }

        public async Task<Models.Contract> UpdateContractUser(Models.Contract contract)
        {
            try
            {

                var contractrt = await _repoCt.UpdateAsync(contract);
                if (contractrt != null)
                {
                    return contractrt;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }
        public async Task<Models.Contract> UpdateContractUserId(int contractId)
        {
            try
            {

                var contractrt = await _repoCt.GetByIdAsync(contractId);
                contractrt.Status = "3";
                var update = await _repoCt.UpdateAsync(contractrt);
                if (contractrt != null)
                {
                    return update;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }
        public async Task<Models.Request> UpdateRequestUser(Models.Request request)
        {
            var re = await _repo.UpdateAsync(request);
            return re;
        }
        
    }
}
