using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Modal.request;
using Quote.Models;
using Quote.Services;
using System.Threading.Tasks;

namespace Quote.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly ITaskInterface _taskService;

        public RequestController(IRequestService requestService, ITaskInterface taskService)
        {
            _requestService = requestService;
            _taskService = taskService;
        }

        [HttpPost("CreateRequest")]
        public async Task<ActionResult<Request>> CreateRequest([FromBody] CreateRequestModel requestdata)
        {
           
            if (requestdata.UserId == null)
            {
                return Unauthorized();
            }
            var request = new Request
            {
                Address = requestdata.Address,
                Email = requestdata.Email,
                ProductId = requestdata.ProductId,
                Date = DateTime.Parse(requestdata.Date),
                Status = requestdata.Status,
                UserId = requestdata.UserId,
                Phone = requestdata.Phone,
                UserName = requestdata.UserName,
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
        [HttpPost("ConfirmAppointment")]
        public async Task<ActionResult> ConfirmAppointment(int requestId, int staffId)
        {
            try
            {
                var request = await _requestService.Appoinment(requestId);
                if(request == null)
                {
                    return NotFound();
                }            
                    Models.Task newTask = new Models.Task();
                    newTask.RequestId = request.RequestId; ;
                    newTask.UserId = staffId;
                    newTask.Status = "0";
                    newTask.Location = request.Address;
                    var item = await _taskService.CreateTasks(newTask);
                    return Ok("Success");
                              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
