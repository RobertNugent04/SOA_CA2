using SOA_CA2.Models;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for managing Comment entity operations.
    /// </summary>
    public interface ICommentRepository
    {
        /// <summary>
        /// Retrieves all comments associated with a post.
        /// </summary>
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);

        /// <summary>
        /// Retrieves a comment by its unique identifier.
        /// </summary>
        Task<Comment?> GetCommentByIdAsync(int commentId);

        /// <summary>
        /// Creates a new comment in the data store.
        /// </summary>
        Task AddCommentAsync(Comment comment);

        /// <summary>
        /// Updates an existing comment in the data store.
        /// </summary>
        Task UpdateCommentAsync(Comment comment);

        /// <summary>
        /// Removes a comment from the data store.
        /// </summary>
        Task DeleteCommentAsync(int commentId);

        /// <summary>
        /// Checks if a comment exists in the data store.
        /// </summary>
        Task<bool> CommentExistsAsync(int commentId);

        /// <summary>
        /// Saves changes to the data store.
        /// </summary>
        Task SaveChangesAsync();
    }
}
