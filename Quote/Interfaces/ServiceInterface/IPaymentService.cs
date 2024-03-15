using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IPaymentService
    {
        Task<PaymentResponse> PayContract(int ContractId, string method, int userId);
        Task<Payment> GetPayContract(int PaymentId);
        System.Threading.Tasks.Task UpdatePay(Payment payment);

        Task<double> TotalMoney();

        Task<List<Payment>> GetAllPay();

        Task<List<RevernueRespone>> RevernueByYear();
    }
}
