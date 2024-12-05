using Microsoft.AspNetCore.SignalR;
using SOA_CA2.Models.DTOs.Notification;

namespace SOA_CA2.SignalR
{
    /// <summary>
    /// SignalR hub for managing real-time notifications.
    /// </summary>
    public class NotificationHub : Hub
    {
        /// <summary>
        /// Sends a notification to a specific user.
        /// </summary>
        public async Task SendNotification(string userId, NotificationDto notification)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }
    }
}
