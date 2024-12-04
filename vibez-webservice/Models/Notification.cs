using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
    /// <summary>
    /// Represents a notification sent to a user in the Vibez application.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Primary key for the Notification entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        /// <summary>
        /// Foreign key referencing the User who receives the notification.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property for the User who receives the notification.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The type of the notification (like FriendRequest, Message, Like, Comment, Call).
        /// </summary>
        [Required]
        [StringLength(50)]
        public required string Type { get; set; }

        /// <summary>
        /// The reference ID linking to the related entity (like PostId, MessageId).
        /// </summary>
        public int? ReferenceId { get; set; }

        /// <summary>
        /// Indicates whether the notification has been read by the user.
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// The message or additional details of the notification.
        /// </summary>
        [Required]
        [StringLength(255)]
        public required string Message { get; set; }

        /// <summary>
        /// The timestamp when the notification was created - UTC.
        /// </summary>
        [Column(TypeName = "timestamptz")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
