using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal.request;
using Quote.Models;

namespace Quote.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestContractController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestContractController(IRequestService requestService)
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
                Status = requestdata.Status,
                UserId = userId,
                //Phone = requestdata.Phone,
            };
            var requestItem = await _requestService.CreateRequestUser(request);
            return Ok("Success");
        }



    }
}
