﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Modal.request;
using Quote.Services;
using System.Text.RegularExpressions;

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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PaymentController(IConfiguration configuration, IPaymentService paymentService, IMapper mapper, IRequestService requestService , IWebHostEnvironment webHostEnvironment, IMailService mailService)
        {
            _configuration = configuration;
            _paymentService = paymentService;
            _mapper = mapper;
            _requestService = requestService;
            _webHostEnvironment = webHostEnvironment;
            _mailService = mailService;
        }
        [HttpPost("Pay")]
        //[Authorize]
        public async Task<ActionResult<PaymentResponse>> CreatePayment([FromForm] PayRequestModel request)
        {
           


            string Filepath = GetContractFile(request.ContractId.ToString());
            if (!Directory.Exists(Filepath))
            {
                Directory.CreateDirectory(Filepath);
            }

            //var base64Data = Regex.Match(request.ContractFile, @"data:image\/[a-zA-Z]+;base64,(?<data>.+)").Groups["data"].Value;
            //var imageBytes = Convert.FromBase64String(request.ContractFile);
            using (var ms = new MemoryStream(request.ContractFile))
            {
                var fileName = Guid.NewGuid().ToString() + ".pdf";
                var filePath = Path.Combine(Filepath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ms.CopyTo(fileStream);

                }
                var datafile = GetContractPath(request.ContractId, fileName);
                var contractItem = await _requestService.GetContractById(request.ContractId);
                contractItem.ContractFile = datafile;

                await _requestService.UpdateContractUser(contractItem);


            }
            var response = await _paymentService.PayContract(request.ContractId, request.Method, request.userId);
            return Ok(response);
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

        [HttpGet("PaymentCallback/{paymentId:int}")]
        public async Task<ActionResult> PaymentCallback([FromRoute] int paymentId, [FromQuery] VnPaymentCallbackModel request)
        {

            if (!request.Success)
            {
                var paymenta = await _paymentService.GetPayContract(paymentId);
                paymenta.Status = "0";
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
            payment.Status = "1";
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
