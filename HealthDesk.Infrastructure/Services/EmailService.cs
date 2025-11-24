using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using HealthDesk.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HealthDesk.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var smtpClient = new SmtpClient(emailSettings["Host"])
            {
                Port = int.Parse(emailSettings["Port"]!),
                Credentials = new NetworkCredential(emailSettings["UserName"], emailSettings["Password"]),
                EnableSsl = bool.Parse(emailSettings["EnableSSL"]!)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings["UserName"]!, "HealthDesk Notifications"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
        }
    }
}
