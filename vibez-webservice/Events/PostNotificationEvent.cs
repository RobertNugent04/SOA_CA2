using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.Notification;

namespace SOA_CA2.Events
{
    /// <summary>
    /// Event handler for creating post-related notifications.
    /// </summary>
    public class PostNotificationEvent : INotificationEvent
    {
        private readonly int _senderId;
        private readonly int _postId;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;

        public PostNotificationEvent(
            int senderId,
            int postId,
            IUnitOfWork unitOfWork,
            INotificationService notificationService,
            ILogger logger)
        {
            _senderId = senderId;
            _postId = postId;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task HandleAsync()
        {
            try
            {
                // Fetch sender's friends who have accepted the request
                IEnumerable<Models.User> friends = await _unitOfWork.Friendships.GetAcceptedFriendsAsync(_senderId);

                foreach (Models.User friend in friends)
                {
                    // Create a notification for each friend
                    await _notificationService.SendNotificationAsync(_senderId, new NotificationCreationDto
                    {
                        UserId = friend.UserId, // Notify each friend
                        Type = "Post",
                        ReferenceId = _postId,
                        Message = "posted something."
                    });
                }

                _logger.LogInformation("Post notifications sent successfully for sender ID: {SenderId}", _senderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending post notifications for sender ID: {SenderId}", _senderId);
                throw;
            }
        }
    }
}
