namespace SOA_CA2.Models.DTOs.Post
{
    /// <summary>
    /// DTO used when creating a new post.
    /// </summary>
    public class PostCreationDto
    {
        /// <summary>
        /// The content of the post.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// The URL of the image associated with the post.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
