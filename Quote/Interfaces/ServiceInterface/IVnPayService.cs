using Quote.Models;

namespace Quote.Interfaces.ServiceInterface

{
    public interface IVnPayService
    {
        string CreatePaymentUrl(Payment payment);

    }
}

