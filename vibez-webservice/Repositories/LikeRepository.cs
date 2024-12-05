using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SOA_CA2.Infrastructure;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;

namespace SOA_CA2.Repositories
{
    /// <summary>
    /// Implements data access methods for the Like entity.
    /// </summary>
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LikeRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeRepository"/> class.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="logger">Logger for debugging and tracking operations.</param>
        public LikeRepository(AppDbContext context, ILogger<LikeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AddLikeAsync(Like like)
        {
            try
            {
                _logger.LogInformation("Adding a like for post ID: {PostId}, user ID: {UserId}.", like.PostId, like.UserId);
                await _context.Likes.AddAsync(like);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding like for post ID: {PostId}, user ID: {UserId}.", like.PostId, like.UserId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task RemoveLikeAsync(int userId, int postId)
        {
            try
            {
                _logger.LogInformation("Removing like for post ID: {PostId}, user ID: {UserId}.", postId, userId);
                var like = await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

                if (like == null)
                {
                    _logger.LogWarning("No like found for post ID: {PostId}, user ID: {UserId}.", postId, userId);
                    return;
                }

                _context.Likes.Remove(like);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing like for post ID: {PostId}, user ID: {UserId}.", postId, userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> LikeExistsAsync(int userId, int postId)
        {
            try
            {
                _logger.LogInformation("Checking if like exists for post ID: {PostId}, user ID: {UserId}.", postId, userId);
                return await _context.Likes.AnyAsync(l => l.UserId == userId && l.PostId == postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking like existence for post ID: {PostId}, user ID: {UserId}.", postId, userId);
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
