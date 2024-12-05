using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.Models.DTOs.PasswordReset
{
    /// <summary>
    /// DTO for resetting a password using an OTP.
    /// </summary>
    public class PasswordResetDto
    {
        /// <summary>
        /// The email address of the user resetting their password.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The OTP sent to the user's email.
        /// </summary>
        [Required]
        public string Otp { get; set; }

        /// <summary>
        /// The new password for the user.
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}
