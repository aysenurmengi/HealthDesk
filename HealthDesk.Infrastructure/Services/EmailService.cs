using System.Net;
using System.Net.Mail;
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

            var host = emailSettings["Host"];
            var userName = emailSettings["UserName"];
            var password = emailSettings["Password"];
            var fromName = emailSettings["FromName"]
                ?? emailSettings["DisplayName"]
                ?? "HealthDesk Notifications";

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Email settings are not configured.");

            var smtpClient = new SmtpClient(host)
            {
                Port = int.Parse(emailSettings["Port"]!),
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = bool.Parse(emailSettings["EnableSSL"]!)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(userName, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
        }
    }
}
