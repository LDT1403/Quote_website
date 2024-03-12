using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
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
        private readonly IMailService _mailService;
        private readonly IProductService _productService;
        private readonly UserInterface userInterface;
        public RequestController(IRequestService requestService, IMailService mailService, IProductService productService, UserInterface userInterface)
        {
            _requestService = requestService;
            _mailService = mailService;
            _productService = productService;
            this.userInterface = userInterface;
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
            var productItem = await _productService.GetProductById( (int)requestItem.ProductId);
            var toEmail = requestItem.Email;

            var emailBody = $@"<div><h3>HỆ THỐNG ĐANG SẮP XẾP NHÂN VIÊN KHẢO SÁT</h3> 
                          <div>
                              <h3>Thông Tin Yêu Cầu</h3>    
                              <span>Tên Người Nhận: </span> <strong>{requestItem.UserName}</strong><br>
                              <span>Số Điện Thoại: </span> <strong>{requestItem.Phone:n0}</strong><br>
                              <span>Địa Chỉ Căn Hộ: </span> <strong>{requestItem.Address}</strong><br>
                              <span>Ngày Khảo Sát: </span> <strong>{requestItem.Date}</strong><br>

                                <h3>Danh Mục Bạn Đã Chọn </h3> 
                              <span>Tên Sản Phẩm: </span> <strong>{productItem.ProductName}</strong><br>
                              
                          </div>
                          <p>Xin trân trọng cảm ơn</p>
                      </div>";
            var mailRequest = new MailRequest()
            {
                ToEmail = toEmail,
                Subject = "[GOAT INTERIOR] CHÚNG TÔI ĐÃ NHẬN ĐƯỢC YÊU CẦU CỦA BẠN!",
                Body = emailBody
            };
            await _mailService.SendEmailAsync(mailRequest);
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
