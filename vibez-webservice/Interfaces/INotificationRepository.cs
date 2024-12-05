using SOA_CA2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for managing data access operations for the Notification entity.
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Adds a new notification to the database.
        /// </summary>
        Task AddNotificationAsync(Notification notification);

        /// <summary>
        /// Retrieves all notifications for a specific user.
        /// </summary>
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        Task UpdateNotificationAsync(Notification notification);

        /// <summary>
        /// Retrieves a specific notification by ID.
        /// </summary>
        Task<Notification?> GetNotificationByIdAsync(int notificationId);

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        Task SaveChangesAsync();
    }
}
