using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.Models.DTOs.User
{
    /// <summary>
    /// DTO used for user login.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// The username or email used for logging in.
        /// </summary>
        [Required]
        public required string UsernameOrEmail { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public required string Password { get; set; }
    }
}
