using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Repositorys;

namespace Quote.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentRepository _repoPay;
        private readonly ContractRepository _repoCont;
        private readonly IVnPayService _serviceVnPay;

        public PaymentService(PaymentRepository repoPay, ContractRepository repoCont, IVnPayService vnPayService )
        {
            _repoPay = repoPay;
            _repoCont = repoCont;
            _serviceVnPay = vnPayService;
        }
    
        public async Task<Payment> GetPayContract(int PaymentId)
        {
            try
            {
                var payment = await _repoPay.GetByIdAsync(PaymentId);
                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }

        public async Task<PaymentResponse> PayContract(int ContractId, string method,int userId)
        {
            var contract = await _repoCont.GetByIdAsync((int)ContractId);
            var pricePay = ((int.Parse(contract.FinalPrice)) + (int.Parse(contract.ConPrice))) * 30/100;
            var paymentdata = new Payment()
            {
                ContractId = contract.ContractId,
                Method = method,
                DatePay = DateTime.Now,
                PricePay = pricePay.ToString(),
                UserId = userId,
            };
            var payment = await _repoPay.AddReturnAsync(paymentdata);
            var paymentUrl = _serviceVnPay.CreatePaymentUrl(payment);
            var payrespon = new PaymentResponse
            {
                PaymentId = payment.PaymentId,
                PricePay = decimal.Parse(payment.PricePay),
                PaymentUrl = paymentUrl,
                DatePay = payment.DatePay,
                Method = method,
                UserId = (int)payment.UserId
            };
            return payrespon;
        }

        public async System.Threading.Tasks.Task UpdatePay(Payment payment)
        {
            
            await _repoPay.UpdateAsync(payment);            
        }
    }
}
