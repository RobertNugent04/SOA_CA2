using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
    /// <summary>
    /// Represents a user in the Vibez social media application.
    /// </summary>
    public class User
	{
        /// <summary>
        /// Primary key for the User entity.
        /// </summary>
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }

        /// <summary>
        /// The full name of the user.
        /// </summary>
        [Required]
		[Column(TypeName = "varchar(150)")] 
		[StringLength(150, MinimumLength = 2)]
		public required string FullName { get; set; }

        /// <summary>
        /// The unique username chosen by the user.
        /// </summary>
        [Required]
		[Column(TypeName = "varchar(50)")] 
		[StringLength(50, MinimumLength = 3)]
		public required string UserName { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        [Required]
		[EmailAddress]
		[Column(TypeName = "varchar(100)")] 
		[StringLength(100)]
		public required string Email { get; set; }

        /// <summary>
        /// The hashed password of the user.
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(255)")]
        public required string PasswordHash { get; set; }

        /// <summary>
        /// A short biography or description provided by the user.
        /// </summary>
        [Column(TypeName = "text")]
		[StringLength(500)] 
		public string? Bio { get; set; }

        /// <summary>
        /// The URL to the user's profile picture.
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string? ProfilePictureUrl { get; set; }

        /// <summary>
        /// Indicates whether the user's account is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// The date and time when the user account was created (in UTC).
        /// </summary>
        [Column(TypeName = "timestamptz")] 
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date and time when the user account was last updated (in UTC).
        /// </summary>
		[Column(TypeName = "timestamptz")] 
		public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Navigation property for the posts created by the user.
        /// </summary>
        public ICollection<Post> Posts { get; set; }

        /// <summary>
        /// Navigation property for the likes given by the user.
        /// </summary>
        public ICollection<Like> Likes { get; set; }

        /// <summary>
        /// Navigation property for the comments made by the user.
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Navigation property for the friendships of the user.
        /// </summary>
        public ICollection<Friendship> Friendships { get; set; }

        /// <summary>
        /// Navigation property for the messages sent by the user.
        /// </summary>
        public ICollection<Message> MessagesSent { get; set; }

        /// <summary>
        /// Navigation property for the messages received by the user.
        /// </summary>
        public ICollection<Message> MessagesReceived { get; set; }

        /// <summary>
        /// Navigation property for the notifications received by the user.
        /// </summary>
        public ICollection<Notification> Notifications { get; set; }

        /// <summary>
        /// Navigation property for the calls made by the user.
        /// </summary>
        public ICollection<Call> CallsMade { get; set; }

        /// <summary>
        /// Navigation property for the calls received by the user.
        /// </summary>
        public ICollection<Call> CallsReceived { get; set; }
    }
}
