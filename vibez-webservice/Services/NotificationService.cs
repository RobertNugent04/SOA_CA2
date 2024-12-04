using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Notification;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Implements business logic for managing notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUnitOfWork unitOfWork, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task SendNotificationAsync(NotificationCreationDto dto)
        {
            try
            {
                _logger.LogInformation("Creating notification for user ID: {UserId}.", dto.UserId);

                Notification notification = new Notification
                {
                    UserId = dto.UserId,
                    Type = dto.Type,
                    ReferenceId = dto.ReferenceId,
                    Message = dto.Message,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Notifications.AddNotificationAsync(notification);
                await _unitOfWork.Notifications.SaveChangesAsync();

                _logger.LogInformation("Notification created successfully.");
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
        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            try
            {
                _logger.LogInformation("Marking notification ID: {NotificationId} as read.", notificationId);

                Notification? notification = await _unitOfWork.Notifications.GetNotificationByIdAsync(notificationId);

                if (notification == null)
                {
                    throw new ArgumentException("Notification not found.");
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
