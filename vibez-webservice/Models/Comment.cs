using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
    /// <summary>
    /// Represents a comment made by a user on a post in the Vibez application.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Primary key for the Comment entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        /// <summary>
        /// Foreign key referencing the Post that this comment belongs to.
        /// </summary>
        [Required]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// Navigation property for the Post that this comment belongs to.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// Foreign key referencing the User who made the comment.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property for the User who made the comment.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The content of the comment.
        /// </summary>
        [Required]
        [StringLength(1000)]
        public required string Content { get; set; }

        /// <summary>
        /// The timestamp when the comment was created - UTC.
        /// </summary>
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
