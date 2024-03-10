using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IPaymentService
    {
        Task<PaymentResponse> PayContract(int ContractId, string method, int userId);
        Task<Payment> GetPayContract(int PaymentId);
    }
}
