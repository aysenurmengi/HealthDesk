using HealthDesk.Application.Common.Interfaces;

namespace HealthDesk.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
             Console.WriteLine($"E-posta gönderildi: To={to}, Subject={subject}, Body={body}");
            return Task.CompletedTask;
        }
    }
}