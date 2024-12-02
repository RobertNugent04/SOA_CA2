using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
    /// <summary>
    /// Represents a post created by a user in the Vibez application.
    /// </summary>
    public class Post
	{
        /// <summary>
        /// Primary key for the Post entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        /// <summary>
        /// Foreign key referencing the User who created the post.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// The content of the post.
        /// </summary>
        [Required]
        [Column(TypeName = "text")]
        public required string Content { get; set; }

        /// <summary>
        /// The URL of the image associated with the post (if any).
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// The timestamp when the post was created (in UTC).
        /// </summary>
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The timestamp when the post was last updated (in UTC).
        /// </summary>
        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; }

    }
}
