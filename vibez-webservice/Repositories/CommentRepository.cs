using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SOA_CA2.Infrastructure;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;

namespace SOA_CA2.Repositories
{
    /// <summary>
    /// Implements data access methods for the Comment entity.
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(AppDbContext context, ILogger<CommentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            try
            {
                _logger.LogInformation("Fetching comments for post ID: {PostId}", postId);
                return await _context.Comments
                    .Where(c => c.PostId == postId)
                    .OrderByDescending(c => c.CreatedAt)
                    .Include(c => c.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching comments for post ID: {PostId}", postId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            try
            {
                _logger.LogInformation("Fetching comment by ID: {CommentId}", commentId);
                return await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.Post)
                    .FirstOrDefaultAsync(c => c.CommentId == commentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching comment by ID: {CommentId}", commentId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task AddCommentAsync(Comment comment)
        {
            try
            {
                _logger.LogInformation("Adding a new comment for post ID: {PostId}", comment.PostId);
                await _context.Comments.AddAsync(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment for post ID: {PostId}", comment.PostId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateCommentAsync(Comment comment)
        {
            try
            {
                _logger.LogInformation("Updating comment ID: {CommentId}", comment.CommentId);
                _context.Comments.Update(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating comment ID: {CommentId}", comment.CommentId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeleteCommentAsync(int commentId)
        {
            try
            {
                _logger.LogInformation("Deleting comment ID: {CommentId}", commentId);
                var comment = await GetCommentByIdAsync(commentId);
                if (comment == null)
                {
                    throw new ArgumentException($"Comment with ID {commentId} not found.");
                }
                _context.Comments.Remove(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment ID: {CommentId}", commentId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> CommentExistsAsync(int commentId)
        {
            try
            {
                return await _context.Comments.AnyAsync(c => c.CommentId == commentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of comment ID: {CommentId}", commentId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync()
        {
            try
            {
                _logger.LogInformation("Saving changes to the database.");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to the database.");
                throw;
            }
        }
    }
}
