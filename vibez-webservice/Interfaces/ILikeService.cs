using SOA_CA2.Models.DTOs.Like;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for Like service to manage like-related operations.
    /// </summary>
    public interface ILikeService
    {
        /// <summary>
        /// Adds a like to a post.
        /// </summary>
        Task AddLikeAsync(int userId, LikeCreationDto dto);

        /// <summary>
        /// Removes a like from a post.
        /// </summary>
        Task RemoveLikeAsync(int userId, int postId);
    }
}
