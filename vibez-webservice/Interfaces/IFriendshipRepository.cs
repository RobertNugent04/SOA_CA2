using SOA_CA2.Models;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Defines the contract for data access operations related to friendships.
    /// </summary>
    public interface IFriendshipRepository
    {
        /// <summary>
        /// Sends a new friend request.
        /// </summary>
        Task AddFriendRequestAsync(Friendship friendship);

        /// <summary>
        /// Gets a friendship by its ID.
        /// </summary>
        Task<Friendship?> GetFriendshipByIdAsync(int friendshipId);

        /// <summary>
        /// Updates a friendship's status.
        /// </summary>
        Task UpdateFriendshipAsync(Friendship friendship);

        /// <summary>
        /// Checks if a friendship already exists between two users.
        /// </summary>
        Task<bool> FriendshipExistsAsync(int userId, int friendId);

        /// <summary>
        /// Retrieves the friendship status between two users.
        /// </summary>
        Task<Friendship?> GetFriendshipBetweenUsersAsync(int userId, int friendId);

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        Task SaveChangesAsync();
    }
}
