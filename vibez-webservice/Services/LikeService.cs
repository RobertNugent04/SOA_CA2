using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Like;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Implements business logic for managing likes.
    /// </summary>
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LikeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <param name="logger">Logger for debugging and tracking operations.</param>
        public LikeService(IUnitOfWork unitOfWork, ILogger<LikeService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AddLikeAsync(int userId, LikeCreationDto dto)
        {
            try
            {
                _logger.LogInformation("Adding like for post ID: {PostId}, user ID: {UserId}.", dto.PostId, userId);

                bool exists = await _unitOfWork.Likes.LikeExistsAsync(userId, dto.PostId);
                if (exists)
                {
                    throw new ArgumentException("Like already exists for this post by the user.");
                }

                Like like = new Like
                {
                    UserId = userId,
                    PostId = dto.PostId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Likes.AddLikeAsync(like);
                await _unitOfWork.Likes.SaveChangesAsync();

                _logger.LogInformation("Like added successfully for post ID: {PostId}, user ID: {UserId}.", dto.PostId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding like for post ID: {PostId}, user ID: {UserId}.", dto.PostId, userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task RemoveLikeAsync(int userId, int postId)
        {
            try
            {
                _logger.LogInformation("Removing like for post ID: {PostId}, user ID: {UserId}.", postId, userId);

                bool exists = await _unitOfWork.Likes.LikeExistsAsync(userId, postId);
                if (!exists)
                {
                    throw new ArgumentException("Like does not exist for this post by the user.");
                }

                await _unitOfWork.Likes.RemoveLikeAsync(userId, postId);
                await _unitOfWork.Likes.SaveChangesAsync();

                _logger.LogInformation("Like removed successfully for post ID: {PostId}, user ID: {UserId}.", postId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing like for post ID: {PostId}, user ID: {UserId}.", postId, userId);
                throw;
            }
        }
    }
}
