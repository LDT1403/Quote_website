using AutoMapper.Internal;
using Quote.Modal.request;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailController);
    }
}
