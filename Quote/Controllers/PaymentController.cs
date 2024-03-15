using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Modal.request;

namespace Quote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentService _paymentService;
        private readonly IRequestService _requestService;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        public PaymentController(IConfiguration configuration, IPaymentService paymentService, IMapper mapper, IRequestService requestService, IMailService mailService)
        {
            _configuration = configuration;
            _paymentService = paymentService;
            _mapper = mapper;
            _requestService = requestService;
            _mailService = mailService;
        }
        [HttpPost("Pay")]
        //[Authorize]
        public async Task<ActionResult<PaymentResponse>> CreatePayment([FromBody] PayRequestModel request)
        {
            var response = await _paymentService.PayContract(request.ContractId, request.Method , request.userId);
            return Ok(response);
        }
        [HttpGet("PaymentCallback/{paymentId:int}")]
        public async Task<ActionResult> PaymentCallback([FromRoute] int paymentId, [FromQuery] VnPaymentCallbackModel request)
        {

            if (!request.Success)
            {
                var paymenta = await _paymentService.GetPayContract(paymentId);
                paymenta.Status = "fail";
                await _paymentService.UpdatePay(paymenta);
                var contractFail = await _requestService.UpdateContractUserId((int)paymenta.ContractId);
                var requestFail = await _requestService.GetRequestById((int)contractFail.RequestId);
                var toEmailFail = requestFail.Email;

                var emailBodyFail = $@"<h2>Xin chào {requestFail.UserName},</h2>
               <h3>Thông Tin Thanh Toán</h3>
               <p>Mã thanh toán: GOATINTERIOR {request.TransactionCode}</p>
               <p>Số tiền thanh toán: {paymenta.PricePay} chưa được thanh toán</p>
               <p>Vui lòng thanh toán lại.</p>";

                var mailError = new MailRequest()
                {
                    ToEmail = toEmailFail,
                    Subject = "[GOAT INTERIOR] THANH TOÁN THẤT BẠI",
                    Body = emailBodyFail,
                };
                await _mailService.SendEmailAsync(mailError);

                return Redirect(_configuration["Payment:Failed"]);
            }

            var payment = await _paymentService.GetPayContract(paymentId);
            payment.Status = "success";
            await _paymentService.UpdatePay(payment);
            var contract = await _requestService.UpdateContractUserId((int)payment.ContractId);
            var re = await _requestService.GetRequestById((int)contract.RequestId);
            var toEmail = re.Email;

            var emailBodySuccess = $@"<h2>Xin chào {re.UserName},</h2>
               <h3>Thông Tin Thanh Toán</h3>
               <p>Mã thanh toán: GOATINTERIOR {request.TransactionCode}</p>
               <p>Số tiền thanh toán: {payment.PricePay}</p>
               <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>";
           
            var mailSuccess = new MailRequest()
            {
                ToEmail = toEmail,
                Subject = "[GOAT INTERIOR] THANH TOÁN THÀNH CÔNG",
                Body = emailBodySuccess,
            };
            
            await _mailService.SendEmailAsync(mailSuccess);
            return Redirect(_configuration["Payment:SuccessUrl"]);

        }

    }
}
