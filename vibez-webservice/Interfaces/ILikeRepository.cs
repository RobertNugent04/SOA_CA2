using SOA_CA2.Models;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for Like repository to manage like-related data access.
    /// </summary>
    public interface ILikeRepository
    {
        /// <summary>
        /// Adds a new like.
        /// </summary>
        Task AddLikeAsync(Like like);

        /// <summary>
        /// Removes a like by user and post.
        /// </summary>
        Task RemoveLikeAsync(int userId, int postId);

        /// <summary>
        /// Checks if a like exists for a user and a post.
        /// </summary>
        Task<bool> LikeExistsAsync(int userId, int postId);

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        Task SaveChangesAsync();
    }
}
