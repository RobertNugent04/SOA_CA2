namespace SOA_CA2.Models.DTOs.Like
{
    /// <summary>
    /// DTO used for transferring like data to the client.
    /// </summary>
    public class LikeDto
    {
        /// <summary>
        /// The unique identifier of the like.
        /// </summary>
        public int LikeId { get; set; }

        /// <summary>
        /// The timestamp when the like was created - UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The ID of the user who liked the post.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The ID of the post that was liked.
        /// </summary>
        public int PostId { get; set; }
    }
}
