using Microsoft.AspNetCore.SignalR;
using SOA_CA2.Models.DTOs.Message;

namespace SOA_CA2.SignalR
{
    /// <summary>
    /// SignalR hub for real-time messaging.
    /// </summary>
    public class MessageHub : Hub
    {
        /// <summary>
        /// Sends a message to a specific user.
        /// </summary>
        public async Task SendMessage(string userId, MessageDto message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", message);
        }
    }
}
