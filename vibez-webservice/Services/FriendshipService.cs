using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Friendship;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Implements business logic for managing friendships.
    /// </summary>
    public class FriendshipService : IFriendshipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FriendshipService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipService"/> class.
        /// </summary>
        public FriendshipService(IUnitOfWork unitOfWork, ILogger<FriendshipService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending friend request from user ID: {UserId} to friend ID: {FriendId}.", userId, dto.FriendId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateFriendshipStatusAsync(int userId, int friendshipId, FriendshipUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Updating status of friendship ID: {FriendshipId} by user ID: {UserId}.", friendshipId, userId);

                Friendship? friendship = await _unitOfWork.Friendships.GetFriendshipByIdAsync(friendshipId);

                if (friendship == null)
                {
                    throw new ArgumentException("Friendship not found.");
                }

                if (friendship.FriendId != userId)
                {
                    throw new UnauthorizedAccessException("User is not authorized to update this friendship.");
                }

                friendship.Status = dto.Status;

                await _unitOfWork.Friendships.UpdateFriendshipAsync(friendship);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Friendship status updated successfully for friendship ID: {FriendshipId}.", friendshipId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating friendship status for friendship ID: {FriendshipId}.", friendshipId);
                throw;
            }
        }
    }
}
