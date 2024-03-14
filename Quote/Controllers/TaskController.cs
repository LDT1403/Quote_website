using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Modal.request;

namespace Quote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskInterface _taskService;
        private readonly IRequestService _requestService;
      
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TaskController(ITaskInterface taskService, IRequestService requestService, IWebHostEnvironment webHostEnvironment )
        {
            _taskService = taskService;
            _requestService = requestService;
            _webHostEnvironment = webHostEnvironment;
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
                var listresponse = new List<TaskStaffResponse>();
                foreach(var task in taskStaff)
                {
                    
                    var request = await _requestService.GetRequestById((int)task.RequestId);
                    var response = new TaskStaffResponse
                    {
                        RequestId = request.RequestId,
                        TaskId = task.TaskId,
                        Address = request.Address,
                        Date = request.Date.ToString(),
                        Email = request.Email,
                        Phone = request.Phone,
                        UserName = request.UserName,

                    };
                    listresponse.Add(response);
                }
                
                return Ok(listresponse);


            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        private string GetContractFile(string code)
        {
            return this._webHostEnvironment.WebRootPath + "\\Upload\\contract\\" + code;
        }
        [NonAction]
        private string GetContractPath(int contractId, string fileName)
        {
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return hosturl + "\\Upload\\contract\\" + contractId + "/" + fileName;
        }
        [HttpPost("CreateContractStaff")]
        public async Task<ActionResult<Models.Contract>> CreateContractStaff([FromForm] CreateContractModel Contractdata)
        {

            var request = await _requestService.GetRequestById((int)Contractdata.RequestId);
            request.Status = "3";
            var reUpdate = await _requestService.UpdateRequestUser(request);
            var task = await _taskService.GetTaskById(Contractdata.taskId);
            task.Status = "2";
            var taskUpdate = await _taskService.UpdateTasks(task);


            var contract = new Models.Contract
            {
                RequestId = Contractdata.RequestId,
                ConPrice = Contractdata.ConPrice,
                FinalPrice = Contractdata.FinalPrice,
                Status = Contractdata.Status,
            };

            var contractres = await _requestService.CreateContractUser(contract);

            string Filepath = GetContractFile(contractres.ContractId.ToString());
            if (!Directory.Exists(Filepath))
            {
                Directory.CreateDirectory(Filepath);
            }
            var fileName = Contractdata.ContractFile.FileName;
            var docPath = Filepath + "\\" + fileName;

            if (System.IO.File.Exists(docPath))
            {
                System.IO.File.Delete(docPath);
            }
            using (FileStream stream = System.IO.File.Create(docPath))
            {
                await Contractdata.ContractFile.CopyToAsync(stream);
                string docxFile = GetContractPath(contractres.ContractId, fileName);

                contractres.ContractFile = docxFile;
                var contdata = await _requestService.UpdateContractUser(contractres);
                return Ok(docxFile);
            }


        }
       
    }
}
