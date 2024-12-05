namespace SOA_CA2.Models.DTOs.Friendship
{
    /// <summary>
    /// DTO used for creating a new friendship (sending a friend request).
    /// </summary>
    public class FriendshipCreationDto
    {
        /// <summary>
        /// The ID of the user who will be the friend.
        /// </summary>
        public int FriendId { get; set; }
    }
}
