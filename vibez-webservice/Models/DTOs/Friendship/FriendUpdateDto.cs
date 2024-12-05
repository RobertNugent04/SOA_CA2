namespace SOA_CA2.Models.DTOs.Friendship
{
    /// <summary>
    /// DTO used for updating the status of a friendship (like Accept, Reject).
    /// </summary>
    public class FriendshipUpdateDto
    {
        /// <summary>
        /// The updated status of the friendship (like Accepted, Rejected).
        /// </summary>
        public required string Status { get; set; }
    }
}
