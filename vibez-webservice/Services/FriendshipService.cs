using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Friendship;
using SOA_CA2.Models.DTOs.Notification;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Implements business logic for managing friendships.
    /// </summary>
    public class FriendshipService : IFriendshipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FriendshipService> _logger;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipService"/> class.
        /// </summary>
        public FriendshipService(IUnitOfWork unitOfWork, ILogger<FriendshipService> logger, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _notificationService = notificationService;
        }

        /// <inheritdoc />
        public async Task SendFriendRequestAsync(int userId, FriendshipCreationDto dto)
        {
            try
            {
                _logger.LogInformation("Sending friend request from user ID: {UserId} to friend ID: {FriendId}.", userId, dto.FriendId);

                bool exists = await _unitOfWork.Friendships.FriendshipExistsAsync(userId, dto.FriendId);
                if (exists)
                {
                    throw new ArgumentException("Friendship request already exists.");
                }

                Friendship friendship = new Friendship
                {
                    UserId = userId,
                    FriendId = dto.FriendId,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Friendships.AddFriendRequestAsync(friendship);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Friend request sent successfully from user ID: {UserId} to friend ID: {FriendId}.", userId, dto.FriendId);

                // Send notification to the recipient of the friend request
                await _notificationService.SendNotificationAsync(userId, new NotificationCreationDto
                {
                    UserId = dto.FriendId,
                    Type = "FriendRequest",
                    ReferenceId = userId, // Reference to the sender's profile
                    Message = "sent you a friend request."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending friend request from user ID: {UserId} to friend ID: {FriendId}.", userId, dto.FriendId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateFriendshipStatusAsync(int userId, int friendId, string status)
        {
            try
            {
                _logger.LogInformation("Updating friendship status for friend ID: {FriendId} by user ID: {UserId}.", friendId, userId);

                Friendship? friendship = await _unitOfWork.Friendships.GetFriendshipBetweenUsersAsync(userId, friendId);

                if (friendship == null)
                {
                    throw new ArgumentException("Friendship not found.");
                }

                if (friendship.FriendId != userId && friendship.UserId != userId)
                {
                    throw new UnauthorizedAccessException("User is not authorized to update this friendship.");
                }

                friendship.Status = status;

                await _unitOfWork.Friendships.UpdateFriendshipAsync(friendship);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Friendship status updated successfully for friend ID: {FriendId}.", friendId);

                if (status == "Accepted")
                {
                    // Send notification to the user whose request was accepted
                    int otherUserId = friendship.UserId == userId ? friendship.FriendId : friendship.UserId;
                    await _notificationService.SendNotificationAsync(userId, new NotificationCreationDto
                    {
                        UserId = otherUserId,
                        Type = "FriendRequestAccepted",
                        ReferenceId = userId, // Reference to the person's profile
                        Message = "accepted your friend request."
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating friendship status for friend ID: {FriendId}.", friendId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<string?> GetFriendshipStatusAsync(int userId, int friendId)
        {
            try
            {
                _logger.LogInformation("Fetching friendship status between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);

                // Check if the friendship exists
                Friendship? friendship = await _unitOfWork.Friendships.GetFriendshipBetweenUsersAsync(userId, friendId);
                if (friendship == null)
                {
                    _logger.LogWarning("No friendship found between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);
                    return null;
                }

                _logger.LogInformation("Friendship status fetched successfully: {Status}.", friendship.Status);
                return friendship.Status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching friendship status between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);
                throw;
            }
        }

    }
}
