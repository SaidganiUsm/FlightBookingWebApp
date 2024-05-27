using FlightBookingApp.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace FlightBookingApp.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async System.Threading.Tasks.Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var smtpServer = _configuration["SmtpSettings:Host"];
            var smtpPort = _configuration["SmtpSettings:Port"];
            var smtpUsername = _configuration["SmtpSettings:Username"];
            var smtpPassword = _configuration["SmtpSettings:Password"];

            var from = new MailAddress(smtpUsername, "MED-APSUZ");
            var to = new MailAddress(toEmail);
            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = content,
                IsBodyHtml = true
            };

            using (var client = new SmtpClient(smtpServer, int.Parse(smtpPort!)))
            {
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;
                await client.SendMailAsync(message);
            }
        }
    }
}
