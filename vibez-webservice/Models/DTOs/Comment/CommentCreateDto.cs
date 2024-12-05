namespace SOA_CA2.Models.DTOs.Comment
{
    /// <summary>
    /// DTO used for creating a new comment.
    /// </summary>
    public class CommentCreationDto
    {
        /// <summary>
        /// The content of the comment.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// The ID of the post the comment belongs to.
        /// </summary>
        public int PostId { get; set; }
    }
}
