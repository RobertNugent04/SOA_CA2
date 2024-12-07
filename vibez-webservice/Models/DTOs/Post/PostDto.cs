namespace SOA_CA2.Models.DTOs.Post
{
    /// <summary>
    /// DTO used for transferring post data to the client.
    /// </summary>
    public class PostDTO
    {
        /// <summary>
        /// The unique identifier of the post.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// The unique identifier of the user who created the post.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The content of the post.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// The URL of the image associated with the post.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// The username of the user who created the post.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The profile picture path of the user who created the post.
        /// </summary>
        public string? ProfilePicturePath { get; set; }

        /// <summary>
        /// The number of likes the post has.
        /// </summary>
        public int LikesCount { get; set; }

        /// <summary>
        /// The timestamp when the post was created UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
