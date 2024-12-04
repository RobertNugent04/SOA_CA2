using SOA_CA2.Models.DTOs.Post;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface defining business logic methods for the Post entity.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Retrieves a paginated list of all posts.
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of posts per page.</param>
        /// <returns>A list of post DTOs.</returns>
        Task<IEnumerable<PostDTO>> GetAllPostsAsync(int pageNumber, int pageSize);

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
