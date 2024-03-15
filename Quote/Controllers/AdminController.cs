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

        public AdminController(IRequestService requestService, UserInterface userInterface, IContractService contractService,IPaymentService paymentService)
        {
            _requestService = requestService;
            _userInterface = userInterface;
            _contractService = contractService;
            _paymentService = paymentService;
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
                modal.TotalStaff =listST?.Where(p => p.Role == "ST").Count().ToString();
                return Ok(modal);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get5Contract")]
        public async Task<IActionResult> Get5Contract()
        {
            try
            {
                var item = await _contractService.GetNewContract();
                return Ok(item);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetRervenueByYear")]
        public async Task<IActionResult> GetRervenueByYear()
        {
            try
            {
                var items = await _paymentService.RevernueByYear();
                return Ok(items);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetContractAdmin")]
        public async Task<IActionResult> GetContractAdmin()
        {
            try
            {
                var list = await _requestService.GetContract();

                return Ok(list);
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
                var request = await _requestService.GetRequestById((int)contract.RequestId);
                request.Status = "4";
                await _requestService.UpdateRequestUser(request);
                return Ok("Success"); 
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message); 
            }

        }

    }
}
