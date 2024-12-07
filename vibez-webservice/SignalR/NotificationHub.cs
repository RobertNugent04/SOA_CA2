using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SOA_CA2.Models.DTOs.Notification;
using System.Security.Claims;

namespace SOA_CA2.SignalR
{
    /// <summary>
    /// SignalR hub for managing real-time notifications.
    /// </summary>
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called when a client connects to the SignalR hub.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("User connected to NotificationHub: {UserId}, ConnectionId: {ConnectionId}", userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when a client disconnects from the SignalR hub.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (exception == null)
            {
                _logger.LogInformation("User disconnected from NotificationHub: {UserId}, ConnectionId: {ConnectionId}", userId, Context.ConnectionId);
            }
            else
            {
                _logger.LogError(exception, "User disconnected from NotificationHub with error: {UserId}, ConnectionId: {ConnectionId}", userId, Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Sends a notification to a specific user.
        /// </summary>
        public async Task SendNotification(string userId, NotificationDto notification)
        {
            _logger.LogInformation("Sending notification to user: {UserId}, Message: {Message}", userId, notification.Message);

            await Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }
    }
}
