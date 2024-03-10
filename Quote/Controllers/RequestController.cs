using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal.request;
using Quote.Models;

namespace Quote.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost("CreateRequest")]
        public async Task<ActionResult<Request>> CreateRequest([FromBody] CreateRequestModel requestdata, int userId)
        {
           
            if (userId == null)
            {
                return Unauthorized();
            }
            var request = new Request
            {
                Address = requestdata.Address,
                Email = requestdata.Email,
                ProductId = requestdata.ProductId,
                Date = DateTime.Now,
                Status = requestdata.Status,
                UserId = userId,
                //Phone = requestdata.Phone,
            };
            var requestItem = await _requestService.CreateRequestUser(request);
            return Ok("Success");
        }
        [HttpPost("CreateContract")]
        public async Task<ActionResult<Request>> CreateContract([FromForm] CreateContractModel Contractdata)
        {
            //if (userId == null)
            //{
            //    return Unauthorized();
            //}
            var contract = new Contract
            {
               RequestId = Contractdata.RequestId,
               ConPrice = Contractdata.ConPrice,
               FinalPrice = Contractdata.FinalPrice,
              // Status = Contractdata.Status,
              //ContractFile =Contractdata.ContractFile,
              //Phone = requestdata.Phone,
            };
            await _requestService.CreateContractUser(contract);
            return Ok("Success");
        }



    }
}
