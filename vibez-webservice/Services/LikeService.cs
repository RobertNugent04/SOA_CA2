using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Like;
using SOA_CA2.Models.DTOs.Notification;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Implements business logic for managing likes.
    /// </summary>
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LikeService> _logger;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <param name="logger">Logger for debugging and tracking operations.</param>
        /// <param name="notificationService">Service for managing notifications.</param>
        public LikeService(IUnitOfWork unitOfWork, ILogger<LikeService> logger, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _notificationService = notificationService;
        }

        /// <inheritdoc />
        public async Task AddLikeAsync(int userId, LikeCreationDto dto)
        {
            try
            {
                _logger.LogInformation("Adding like for post ID: {PostId}, user ID: {UserId}.", dto.PostId, userId);

                // Check if the like already exists
                bool exists = await _unitOfWork.Likes.LikeExistsAsync(userId, dto.PostId);
                if (exists)
                {
                    throw new ArgumentException("Like already exists for this post by the user.");
                }

                // Fetch the post to determine the owner
                Post? post = await _unitOfWork.Posts.GetPostByIdAsync(dto.PostId);
                if (post == null)
                {
                    throw new ArgumentException("Post not found.");
                }

                // Avoid sending notification if the user likes their own post
                if (post.UserId != userId)
                {
                    await _notificationService.SendNotificationAsync(userId, new NotificationCreationDto
                    {
                        UserId = post.UserId, // Post owner's ID
                        Type = "Post",
                        ReferenceId = dto.PostId, // Post ID
                        Message = "liked your post."
                    });
                }

                // Add the like
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

                // Check if the like exists
                bool exists = await _unitOfWork.Likes.LikeExistsAsync(userId, postId);
                if (!exists)
                {
                    throw new ArgumentException("Like does not exist for this post by the user.");
                }

                // Remove the like
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
