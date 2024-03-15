using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Quote.Interfaces.RepositoryInterface;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Repositorys;
using System.Globalization;

namespace Quote.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepoBase<Payment> _repoPay;
        private readonly IRepoBase<Models.Contract> _repoCont;
        private readonly IVnPayService _serviceVnPay;

        public PaymentService(IRepoBase<Payment> repoPay, IRepoBase<Models.Contract> repoCont, IVnPayService vnPayService )
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
            double pricePay = ((int.Parse(contract.FinalPrice)) + (int.Parse(contract.ConPrice))) * 0.3;

            // Tính phần payPrice chỉ lấy phần nguyên
            int payPrice = (int)Math.Floor(pricePay);

            var paymentdata = new Payment()
            {
                ContractId = contract.ContractId,
                Method = method,
                DatePay = DateTime.Now,
                PricePay = payPrice.ToString(),
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

        public async Task<List<RevernueRespone>> RevernueByYear()
        {
            try
            {
                List<RevernueRespone> monthlyProfits = new List<RevernueRespone>();

                for (int i = 1; i <= 12; i++)
                {
                    RevernueRespone profit = new RevernueRespone();
                    double totalMoney = 0;
                    var payments = await _repoPay.GetAllAsync();
                    payments =payments.Where(p=>p.Status == "1").ToList();

                    foreach (var payment in payments)
                    {
                        var mm = payment?.DatePay.Value.Month;
                        var yy = payment?.DatePay.Value.Year;
                        if (mm == i && yy == DateTime.Now.Year)
                        {
                            totalMoney += double.Parse(payment.PricePay);
                        }
                    }

                    profit.Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3);
                    profit.Revernue = totalMoney.ToString();
                    monthlyProfits.Add(profit);
                }

                return monthlyProfits;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<double> TotalMoney()
        {
            try
            {
                double totalMoney = 0;
                var items = await _repoPay.GetAllAsync();
                items =items.Where(p => p.Status == "1").ToList();
                foreach (var item1 in items)
                {
                    totalMoney += double.Parse(item1.PricePay);
                    
                }
                return totalMoney;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Payment>> GetAllPay()
        {
            try
            {
                var payment = await _repoPay.GetAllAsync();
                return payment;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async System.Threading.Tasks.Task UpdatePay(Payment payment)
        {
            
            await _repoPay.UpdateAsync(payment);            
        }
    }
}
