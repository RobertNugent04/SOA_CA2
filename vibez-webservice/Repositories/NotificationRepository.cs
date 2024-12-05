using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SOA_CA2.Infrastructure;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOA_CA2.Repositories
{
    /// <summary>
    /// Implements data access methods for the Notification entity.
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NotificationRepository> _logger;

        public NotificationRepository(AppDbContext context, ILogger<NotificationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                _logger.LogInformation("Adding a new notification for user ID: {UserId}.", notification.UserId);
                await _context.Notifications.AddAsync(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new notification for user ID: {UserId}.", notification.UserId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching notifications for user ID: {UserId}.", userId);
                return await _context.Notifications
                    .Where(n => n.UserId == userId)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notifications for user ID: {UserId}.", userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateNotificationAsync(Notification notification)
        {
            try
            {
                _logger.LogInformation("Updating notification ID: {NotificationId}.", notification.NotificationId);
                _context.Notifications.Update(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating notification ID: {NotificationId}.", notification.NotificationId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Notification?> GetNotificationByIdAsync(int notificationId)
        {
            try
            {
                _logger.LogInformation("Fetching notification by ID: {NotificationId}.", notificationId);
                return await _context.Notifications.FindAsync(notificationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notification by ID: {NotificationId}.", notificationId);
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
