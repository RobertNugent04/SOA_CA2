namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Provides methods to send emails.
    /// </summary>
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string email, string otp);
    }
}
