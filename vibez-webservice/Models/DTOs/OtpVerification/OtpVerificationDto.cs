using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.Models.DTOs.OtpVerification
{
    public class OtpVerificationDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Otp { get; set; }
    }

}
