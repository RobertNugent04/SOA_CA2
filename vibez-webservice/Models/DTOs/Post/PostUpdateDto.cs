namespace SOA_CA2.Models.DTOs.Post
{
    /// <summary>
    /// DTO used when updating an existing post.
    /// </summary>
    public class PostUpdateDto
    {
        /// <summary>
        /// The updated content of the post.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// The updated URL of the image associated with the post.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
