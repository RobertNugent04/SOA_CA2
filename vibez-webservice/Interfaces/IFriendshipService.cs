using SOA_CA2.Models.DTOs.Friendship;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Defines the business logic contract for managing friendships.
    /// </summary>
    public interface IFriendshipService
    {
        /// <summary>
        /// Sends a friend request.
        /// </summary>
        Task SendFriendRequestAsync(int userId, FriendshipCreationDto dto);

        /// <summary>
        /// Updates the status of a friendship (e.g., Accept/Reject).
        /// </summary>
        Task UpdateFriendshipStatusAsync(int userId, int friendshipId, FriendshipUpdateDto dto);
    }
}
