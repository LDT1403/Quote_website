using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;

namespace Quote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskInterface _taskService;
        private readonly IRequestService _requestService;

        public TaskController(ITaskInterface taskService)
        {
            _taskService = taskService;
        }
        [HttpGet("GetAllTask")]
        public async Task<IActionResult> GetAllTask()
        {
            try
            {
                var list = _taskService.GetTasks();
                if(list == null)
                {
                    return BadRequest();
                }
                return Ok(list);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("CreateTask")]
        //public async Task<IActionResult> CreateTask(int requestId, int staffId, [FromBody]TaskReq task)
        //{
        //    try
        //    {
        //        var request = await _requestService.Appoinment(requestId);
                
        //        if(request != null)
        //        {
        //            Models.Task newTask = new Models.Task();
        //            newTask.RequestId = request.RequestId; ;
        //            newTask.UserId = staffId;
        //            newTask.TaskName = task.TaskName;
        //            newTask.Status = "0";
        //            newTask.Location = request.Address;
        //            var item = await _taskService.CreateTasks(newTask);
        //            return Ok("Success");
        //        }
        //        return NotFound();
              

        //    }catch (Exception ex)
        //    {
        //        return BadRequest($"{ex.Message}");
        //    }
        //}
    }
}
