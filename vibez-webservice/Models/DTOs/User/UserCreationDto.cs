using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.Models.DTOs.User
{
    /// <summary>
    /// DTO used for creating a new user account.
    /// </summary>
    public class UserCreationDto
    {
        /// <summary>
        /// The full name of the user.
        /// </summary>
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public required string FullName { get; set; }

        /// <summary>
        /// The desired username.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string UserName { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        /// <summary>
        /// The password chosen by the user.
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}
