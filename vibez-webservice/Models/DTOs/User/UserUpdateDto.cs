using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.Models.DTOs.User
{
    /// <summary>
    /// DTO used for updating existing user information.
    /// </summary>
    public class UserUpdateDto
    {
        /// <summary>
        /// The new full name of the user.
        /// </summary>
        [StringLength(150, MinimumLength = 2)]
        public string? FullName { get; set; }

        /// <summary>
        /// A new biography or description for the user.
        /// </summary>
        [StringLength(500)]
        public string? Bio { get; set; }

        /// <summary>
        /// The profile picture file to be uploaded.
        /// </summary>
        public IFormFile? ProfilePicture { get; set; }
    }
}
