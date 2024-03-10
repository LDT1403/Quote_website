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
                Status = requestdata.Status,
                UserId = userId,
                Phone = requestdata.Phone,
            };
            var requestItem = await _requestService.CreateRequestUser(request);
            return Ok("Success");
        }

        [HttpGet("GetAllRequest")]
        public async Task<ActionResult> GetAllRequest()
        {
            try
            {
                var list = await _requestService.GetAllRequest();
                return Ok(list);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }                             
        }
        [HttpPut("ConfirmAppointment/{requestId}")]
        public async Task<ActionResult> ConfirmAppointment(int requestId)
        {
            try
            {
                var re = await _requestService.Appoinment(requestId);
                if(re == null)
                {
                    return NotFound();
                }
                return Ok(re);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
