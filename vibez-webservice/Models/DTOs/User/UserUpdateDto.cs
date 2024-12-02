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
        public string? Full_Name { get; set; }

        /// <summary>
        /// A new biography or description for the user.
        /// </summary>
        [StringLength(500)]
        public string? Bio { get; set; }

        /// <summary>
        /// The URL to the new profile picture.
        /// </summary>
        [StringLength(255)]
        public string? ProfilePictureUrl { get; set; }
    }
}
