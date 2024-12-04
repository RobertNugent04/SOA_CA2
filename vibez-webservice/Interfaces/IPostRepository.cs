using SOA_CA2.Models;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for managing posts in the data store.
    /// </summary>
    public interface IPostRepository
    {
        /// <summary>
        /// Retrieves all posts created by a user.
        /// </summary>
        Task<IEnumerable<Post>> GetAllPostsAsync(int userId, int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves a post by its unique identifier.
        /// </summary>
        Task<Post?> GetPostByIdAsync(int postId);

        /// <summary>
        /// Adds a new post to the data store.
        /// </summary>
        Task AddPostAsync(Post post);

        /// <summary>
        /// Updates an existing post in the data store.
        /// </summary>
        Task UpdatePostAsync(Post post);

        /// <summary>
        /// Removes a post from the data store.
        /// </summary>
        Task DeletePostAsync(int postId);

        /// <summary>
        /// Checks if a post exists in the data store.
        /// </summary>
        Task<bool> PostExistsAsync(int postId);

        /// <summary>
        /// Saves changes to the data store.
        /// <summary>
        Task SaveChangesAsync();
    }
}
