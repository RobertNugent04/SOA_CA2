using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Notification;
using SOA_CA2.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Implements business logic for managing notifications and integrates with SignalR.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task SendNotificationAsync(int senderId, NotificationCreationDto dto)
        {
            try
            {
                _logger.LogInformation("Creating notification from sender ID: {SenderId} to user ID: {UserId}.", senderId, dto.UserId);

                // Fetch sender name
                User? sender = await _unitOfWork.Users.GetUserByIdAsync(senderId);
                if (sender == null) throw new ArgumentException("Sender not found.");

                Notification notification = new Notification
                {
                    UserId = dto.UserId,
                    SenderId = senderId,
                    Type = dto.Type,
                    ReferenceId = dto.ReferenceId,
                    Message = $"{sender.UserName} {dto.Message}",
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Notifications.AddNotificationAsync(notification);
                await _unitOfWork.Notifications.SaveChangesAsync();

                // Notify the user in real-time via SignalR
                await _hubContext.Clients.User(dto.UserId.ToString()).SendAsync("ReceiveNotification", new NotificationDto
                {
                    NotificationId = notification.NotificationId,
                    SenderId = senderId,
                    SenderName = sender.UserName,
                    Type = notification.Type,
                    ReferenceId = notification.ReferenceId,
                    Message = notification.Message,
                    IsRead = notification.IsRead,
                    CreatedAt = notification.CreatedAt
                });

                _logger.LogInformation("Notification created and sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification for user ID: {UserId}.", dto.UserId);
                throw;
            }
        }


        /// <inheritdoc />
        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching notifications for user ID: {UserId}.", userId);

                IEnumerable<Notification> notifications = await _unitOfWork.Notifications.GetUserNotificationsAsync(userId);

                return notifications.Select(n => new NotificationDto
                {
                    NotificationId = n.NotificationId,
                    SenderId = n.SenderId,
                    Type = n.Type,
                    ReferenceId = n.ReferenceId,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notifications for user ID: {UserId}.", userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task MarkNotificationAsReadAsync(int userId, int notificationId)
        {
            try
            {
                _logger.LogInformation("Marking notification ID: {NotificationId} as read.", notificationId);

                Notification? notification = await _unitOfWork.Notifications.GetNotificationByIdAsync(notificationId);

                if (notification == null)
                {
                    throw new ArgumentException("Notification not found.");
                }

                if (notification.UserId != userId)
                {
                    _logger.LogWarning("User ID: {UserId} is not authorized to mark notification ID: {NotificationId} as read.", userId, notificationId);
                    throw new UnauthorizedAccessException("You are not authorized to mark this notification as read.");
                }

                notification.IsRead = true;

                await _unitOfWork.Notifications.UpdateNotificationAsync(notification);
                await _unitOfWork.Notifications.SaveChangesAsync();

                _logger.LogInformation("Notification marked as read successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification ID: {NotificationId} as read.", notificationId);
                throw;
            }
        }
    }
}