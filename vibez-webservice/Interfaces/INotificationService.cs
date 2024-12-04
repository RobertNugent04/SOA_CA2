using SOA_CA2.Models.DTOs.Notification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for managing notification-related business logic.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Creates and sends a new notification.
        /// </summary>
        Task SendNotificationAsync(int senderId, NotificationCreationDto dto);

        /// <summary>
        /// Retrieves all notifications for a user.
        /// </summary>
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId);

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        Task MarkNotificationAsReadAsync(int notificationId);
    }
}
