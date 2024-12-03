using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
    /// <summary>
    /// Represents a friendship between two users in the Vibez application.
    /// </summary>
    public class Friendship
    {
        /// <summary>
        /// Primary key for the Friendship entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendshipId { get; set; }

        /// <summary>
        /// Foreign key referencing the user who initiated the friendship.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property for the user who initiated the friendship.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Foreign key referencing the user who is the friend.
        /// </summary>
        [Required]
        [ForeignKey("Friend")]
        public int FriendId { get; set; }

        /// <summary>
        /// Navigation property for the user who is the friend.
        /// </summary>
        public User Friend { get; set; }

        /// <summary>
        /// The status of the friendship (like Pending, Accepted, Rejected).
        /// </summary>
        [Required]
        [StringLength(50)]
        public required string Status { get; set; }

        /// <summary>
        /// The timestamp when the friendship was created - UTC.
        /// </summary>
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
