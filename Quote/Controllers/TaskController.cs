using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;

namespace Quote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskInterface _taskService;

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
    }
}
