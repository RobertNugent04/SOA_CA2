namespace SOA_CA2.Models.DTOs.Friendship
{
    /// <summary>
    /// DTO used for transferring friendship data to the client.
    /// </summary>
    public class FriendshipDto
    {
        /// <summary>
        /// The unique identifier of the friendship.
        /// </summary>
        public int FriendshipId { get; set; }

        /// <summary>
        /// The ID of the user who initiated the friendship.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The ID of the user who is the friend.
        /// </summary>
        public int FriendId { get; set; }

        /// <summary>
        /// The current status of the friendship (like Pending, Accepted, Rejected).
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// The timestamp when the friendship was created - UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
