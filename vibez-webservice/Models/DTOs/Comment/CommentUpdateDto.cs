namespace SOA_CA2.Models.DTOs.Comment
{
    /// <summary>
    /// DTO used for updating an existing comment.
    /// </summary>
    public class CommentUpdateDto
    {
        /// <summary>
        /// The updated content of the comment.
        /// </summary>
        public string? Content { get; set; }
    }
}
