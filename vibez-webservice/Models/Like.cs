using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
    /// <summary>
    /// Represents a 'like' given by a user to a post in the Vibez application.
    /// </summary>
    public class Like
    {
        /// <summary>
        /// Primary key for the Like entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LikeId { get; set; }

        /// <summary>
        /// Foreign key referencing the Post that was liked.
        /// </summary>
        [Required]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// Navigation property for the Post that was liked.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// Foreign key referencing the User who liked the post.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property for the User who liked the post.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The timestamp when the like was created - UTC.
        /// </summary>
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
