namespace SOA_CA2.Models.DTOs.Comment
{
    /// <summary>
    /// DTO used for transferring comment data to the client.
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// The unique identifier of the comment.
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// The content of the comment.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// The timestamp when the comment was created - UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The ID of the user who made the comment.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The ID of the post the comment belongs to.
        /// </summary>
        public int PostId { get; set; }
    }
}
