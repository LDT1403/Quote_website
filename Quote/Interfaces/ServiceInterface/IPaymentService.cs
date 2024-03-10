using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IPaymentService
    {
        Task<PaymentResponse> PayContract(int ContractId, string method);
        Task<Payment> GetPayContract(int PaymentId);
    }
}
