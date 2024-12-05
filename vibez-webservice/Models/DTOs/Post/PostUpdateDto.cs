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
        /// The updated file to be associated with the post.
        /// </summary>
        public IFormFile? ImageUrl { get; set; }
    }
}
