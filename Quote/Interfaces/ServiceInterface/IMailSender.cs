using AutoMapper.Internal;
using Quote.Modal.request;

namespace Quote.Interfaces.ServiceInterface
{
    public interface IMailSender
    {
        Task SendEmailAsync(MailRequest mailController);
    }
}
