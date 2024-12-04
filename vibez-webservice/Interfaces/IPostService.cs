using SOA_CA2.Models.DTOs.Post;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface defining business logic methods for the Post entity.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Retrieves all posts created by a user.
        /// </summary>
        Task<IEnumerable<PostDTO>> GetPostsAsync(int userId, int pageNumber, int pageSize);

        /// <summary>
        /// Gets a post by its unique identifier.
        /// </summary>
        Task<PostDTO?> GetPostByIdAsync(int postId);

        /// <summary>
        /// Creates a new post in the data store.
        /// </summary>
        Task CreatePostAsync(int userId, PostCreationDto dto, string? imagePath);

        /// <summary>
        /// Updates an existing post in the data store.
        /// </summary>
        Task UpdatePostAsync(int userId, int postId, PostUpdateDto dto, string? imagePath);

        /// <summary>
        /// Removes a post from the data store.
        /// </summary>
        Task DeletePostAsync(int userId, int postId);
    }
}
