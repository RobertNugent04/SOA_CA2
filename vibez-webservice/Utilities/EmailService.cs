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
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendOtpEmailAsync(string email, string otp)
        {
            try
            {
                using SmtpClient smtpClient = new SmtpClient(_configuration["Email:SmtpHost"])
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
                _logger.LogInformation("OTP email sent successfully to {Email}.", email);
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "SMTP error while sending OTP to {Email}.", email);
                throw new InvalidOperationException("Failed to send OTP email due to SMTP error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending OTP to {Email}.", email);
                throw new InvalidOperationException("Failed to send OTP email.", ex);
            }
        }
    }
}
