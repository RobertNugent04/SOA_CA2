using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.Models.DTOs.PasswordReset
{
    /// <summary>
    /// DTO for requesting a password reset.
    /// </summary>
    public class PasswordResetRequestDto
    {
        /// <summary>
        /// The email address of the user requesting a password reset.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
