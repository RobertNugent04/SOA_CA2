namespace SOA_CA2.Models.DTOs.User
{
    /// <summary>
    /// DTO for transferring user data to the client.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The full name of the user.
        /// </summary>
        public required string FullName { get; set; }

        /// <summary>
        /// The username of the user.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// A short biography or description provided by the user.
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// The Path to the user's profile picture.
        /// </summary>
        public string? ProfilePicturePath { get; set; }

        /// <summary>
        /// The date and time when the user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Indicates whether the user's account is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
