using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal.request;

namespace Quote.Services
{
    public class MailService : IMailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public MailService(IConfiguration configuration)
        {
            _host = configuration["MailSettings:Host"];
            _port = configuration["MailSettings:From"] != null ? int.Parse(configuration["MailSettings:From"]) : 587;
            _username = configuration["MailSettings:Mail"];
            _password = configuration["MailSettings:Password"];
        }
        public async Task SendEmailAsync(MailRequest mailController)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_username);
            email.To.Add(MailboxAddress.Parse(mailController.ToEmail));
            email.Subject = mailController.Subject;
            var builder = new BodyBuilder();

            builder.HtmlBody = mailController.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_host, _port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_username, _password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
