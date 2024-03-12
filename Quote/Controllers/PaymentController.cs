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
        private readonly IMapper _mapper;
        public PaymentController(IConfiguration configuration, IPaymentService paymentService, IMapper mapper, IRequestService requestService)
        {
            _configuration = configuration;
            _paymentService = paymentService;
            _mapper = mapper;
            _requestService = requestService;
        }
        [HttpPost("Pay")]
        //[Authorize]
        public async Task<ActionResult<PaymentResponse>> CreatePayment([FromBody] PayRequestModel request,int userId)
        {
            var response = await _paymentService.PayContract(request.ContractId, request.Method , userId);
            return Ok(response);
        }

        [HttpGet("PaymentCallback/{paymentId:int}")]
        public async Task<ActionResult> PaymentCallback([FromRoute] int paymentId, [FromQuery] VnPaymentCallbackModel request)
        {
          
            if (!request.Success)
            {
                var paymenta = await _paymentService.GetPayContract(paymentId);
                return Redirect(_configuration["Payment:Failed"]);
            }
            var payment = await _paymentService.GetPayContract(paymentId);
            payment.Status = "success";
            await _paymentService.UpdatePay(payment);
            await _requestService.UpdateContractUserId((int)payment.ContractId);

            return Redirect(_configuration["Payment:SuccessUrl"]);

        }
       
    }
}
