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

        public async Task<PaymentResponse> PayContract(int ContractId, string method)
        {
            var contract = await _repoCont.GetByIdAsync(ContractId);
            var pricePay = (int.Parse(contract.FinalPrice) + int.Parse(contract.ConPrice)) * 0.3;
            var paymentdata = new Payment()
            {
                ContractId = contract.ContractId,
                Method = method,
                DatePay = DateTime.Now,
                PricePay = pricePay.ToString(),
                UserId = contract.Request.UserId,
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
    }
}
