using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Services;

namespace Quote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly UserInterface _userInterface;
        private readonly IContractService _contractService;
        private readonly IPaymentService _paymentService;
        private readonly ITaskInterface _taskInterface;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IRequestService requestService, UserInterface userInterface, ITaskInterface taskInterface, IContractService contractService, IPaymentService paymentService, IWebHostEnvironment webHostEnvironment)

        {
            _webHostEnvironment = webHostEnvironment;
            _requestService = requestService;
            _userInterface = userInterface;
            _paymentService = paymentService;
            _contractService = contractService;
            _taskInterface = taskInterface;
        }

        [HttpGet("GetTotalMoney")]
        public async Task<IActionResult> GetTotalMoney()
        {
            try
            {
                PayModal modal = new PayModal();

                var list = await _contractService.GetAllContractsAsync();
                double money = await _paymentService.TotalMoney();
                var listST = await _userInterface.GetUsersAsync();
                modal.TotalMoney = money.ToString();
                modal.Contract = list?.Count().ToString();
                modal.TotalStaff = listST?.Where(p => p.Role == "ST").Count().ToString();
                return Ok(modal);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRervenueByYear()
        {
            try
            {
                var items = await _paymentService.RevernueByYear();
                return Ok(items);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetContractAdmin")]
        public async Task<IActionResult> GetContractAdmin(string status)
        {
            try
            {
                var list = await _requestService.GetContract();
                var listResponse = list.Where(p => p.Status == status).ToList();


                return Ok(listResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

        [HttpPut("AddHR")]
        public async Task<IActionResult> AddHR([FromForm] HRModal hr)
        {
            try
            {

                var satff = new User
                {
                    Email = hr.Email,
                    Phone = hr.Phone,
                    UserName = hr.UserName,
                    Position = hr.Position,
                    Password = hr.Password,
                };
                var hrre = await _userInterface.RegisterStaffAsync(satff);
                string Filepath = GetContractFile(hrre.UserId.ToString());
                if (!Directory.Exists(Filepath))
                {
                    Directory.CreateDirectory(Filepath);
                }
                var fileName = hr.Image.FileName;
                var docPath = Filepath + "\\" + fileName;

                if (System.IO.File.Exists(docPath))
                {
                    System.IO.File.Delete(docPath);
                }
                using (FileStream stream = System.IO.File.Create(docPath))
                {
                    await hr.Image.CopyToAsync(stream);
                    string docxFile = GetContractPath(hrre.UserId, fileName);

                    hrre.Image = docxFile;
                    var staffdata = await _userInterface.UpdateStatusStaff(hrre);
                    return Ok(docxFile);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("AddStaff")]
        public async Task<IActionResult> AddStaff([FromForm] StaffModal staff)
        {
            try
            {

                var satff = new User
                {
                    Email = staff.Email,
                    Phone = staff.Phone,
                    UserName = staff.UserName,
                    Position = staff.Position,
                    ManagerId = staff.ManageId,
                    Password = staff.Password,
                };
                var staffre = await _userInterface.RegisterStaffAsync(satff);
                string Filepath = GetContractFile(staffre.UserId.ToString());
                if (!Directory.Exists(Filepath))
                {
                    Directory.CreateDirectory(Filepath);
                }
                var fileName = staff.Image.FileName;
                var docPath = Filepath + "\\" + fileName;

                if (System.IO.File.Exists(docPath))
                {
                    System.IO.File.Delete(docPath);
                }
                using (FileStream stream = System.IO.File.Create(docPath))
                {
                    await staff.Image.CopyToAsync(stream);
                    string docxFile = GetContractPath(staffre.UserId, fileName);

                    staffre.Image = docxFile;
                    var staffdata = await _userInterface.UpdateStatusStaff(staffre);
                    return Ok(docxFile);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ConfirmContract")]
        public async Task<IActionResult> ConfirmContract(int contractId)
        {
            try
            {

                var contract = await _requestService.GetContractById(contractId);
                contract.Status = "2";
                await _requestService.UpdateContractUser(contract);
                var request = await _requestService.GetRequestById((int)contract.RequestId);
                request.Status = "4";
                await _requestService.UpdateRequestUser(request);
                var task = await _taskInterface.GetTasks();
                var taskList = task.ToList().Where(t => t.RequestId == request.RequestId).FirstOrDefault();
                taskList.Status = "2";
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetAllHR")]
        public async Task<IActionResult> GetAllHR()
        {
            try
            {
                var alllUser = await _userInterface.GetUsersAsync();
                var userList = alllUser.Where(user => user.Role == "ST" && user.ManagerId == null)
                               .Select(user => new StaffModalResponse
                               {
                                   UserName = user.UserName,
                                   Email = user.Email,
                                   Phone = user.Phone,
                                   Position = user.Position,
                                   Image = user.Image,
                                   IsDelete = user.IsDelete,
                                   Id = user.UserId,
                               })
                               .ToList();

                return Ok(userList);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllStaff")]
        public async Task<IActionResult> GetAllStaff()
        {
            try
            {
                var alllUser = await _userInterface.GetUsersAsync();
                var userList = alllUser.Where(user => user.Role == "ST" && user.ManagerId != null)
                               .Select(user => new StaffModalResponse
                               {
                                   UserName = user.UserName,
                                   Email = user.Email,
                                   Phone = user.Phone,
                                   Position = user.Position,
                                   Image = user.Image,
                                   IsDelete = user.IsDelete

                               })
                               .ToList();

                return Ok(userList);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*[HttpPut("UpdateBanStaff")]
        public async Task<IActionResult> UpdateBanStaff(int staffId)
        {
            try
            {
                var staff = await _userInterface.GetStaffByStatus(staffId);

                
                return Ok("success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}
