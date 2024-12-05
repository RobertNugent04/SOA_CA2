using SOA_CA2.Models.DTOs.Comment;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for managing comment-related business logic.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Retrieves all comments associated with a post.
        /// </summary>
        Task<IEnumerable<CommentDto>> GetCommentsForPostAsync(int postId);

        /// <summary>
        /// Creates a new comment in the data store.
        /// </summary>
        Task CreateCommentAsync(int userId, CommentCreationDto dto);

        /// <summary>
        /// Updates an existing comment in the data store.
        /// </summary>
        Task UpdateCommentAsync(int userId, int commentId, CommentUpdateDto dto);

        /// <summary>
        /// Removes a comment from the data store.
        /// </summary>
        Task DeleteCommentAsync(int userId, int commentId);
    }
}
