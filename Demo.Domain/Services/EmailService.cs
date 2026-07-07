using Demo.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Demo.Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendOTPAsync(string toEmail, string otpCode)
        {
            var smtp = _configuration.GetSection("Smtp");
            var host = smtp["Host"];
            var port = int.Parse(smtp["Port"] ?? "587");
            var username = smtp["Username"];
            var password = smtp["Password"];
            var fromEmail = smtp["FromEmail"];

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };
            var message = new MailMessage(fromEmail!, toEmail)
            {
                Subject = "Your login OTP code",
                Body = $"Your login OTP code is: {otpCode}\nCode will expire in 10 minutes.",
                IsBodyHtml = false
            };
            await client.SendMailAsync(message);
        }
    }
}
