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

        [HttpGet("AllTaskByStaffId")]
        public async Task<IActionResult> GetAllTaskByStaffId(int staffId)
        {
            try
            {
                var tasks= await _taskService.GetTasks();
                var taskStaff = tasks.Where(t => t.UserId == staffId).ToList();
                
                
                return Ok(tasks);


            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

    }
}
