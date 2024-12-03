using System.Net;
using System.Net.Mail;
using SOA_CA2.Interfaces;

namespace SOA_CA2.Utilities
{
    /// <summary>
    /// To send OTP emails to users.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOtpEmailAsync(string email, string otp)
        {
            SmtpClient smtpClient = new SmtpClient(_configuration["Email:SmtpHost"])
            {
                Port = int.Parse(_configuration["Email:SmtpPort"]),
                Credentials = new NetworkCredential(
                    _configuration["Email:Username"],
                    _configuration["Email:Password"]),
                EnableSsl = true
            };  

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Email:From"]),
                Subject = "Vibez OTP Verification",
                Body = $"Your OTP is: {otp}",
                IsBodyHtml = false
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
