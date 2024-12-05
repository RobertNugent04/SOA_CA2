namespace SOA_CA2.Models.DTOs.Like
{
    /// <summary>
    /// DTO used for creating a new like.
    /// </summary>
    public class LikeCreationDto
    {
        /// <summary>
        /// The ID of the post to be liked.
        /// </summary>
        public int PostId { get; set; }
    }
}
