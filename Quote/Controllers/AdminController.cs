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

        public AdminController(IRequestService requestService)
        {
            _requestService = requestService;
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
        /*[HttpGet("GetStaff")]
        public async Task<IActionResult> GetStaff()
        {
            return null;
        }*/

        [HttpPut("AddStaff")]
        public async Task<IActionResult> AddStaff([FromBody]  StaffModal staff)
        {
            try
            {
                var satff = new User
                {
                    Email = staff.Email,
                    Phone = staff.Phone,
                    UserName = staff.UserName,
                    Position = staff.Position,
                };
                var item = await _userInterface.RegisterStaffAsync(satff);
                if(item == null)
                {
                    return Ok("Email đã tồn tại");
                }
                return Ok("Success");
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ConfirmContract")]
        public async Task<IActionResult> ConfirmContract(int contractId)
        {
            try {
                
                var contract = await _requestService.GetContractById(contractId);
                contract.Status = "2";
                await _requestService.UpdateContractUser(contract);
                return Ok("Success"); 
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message); 
            }

        }

    }
}
