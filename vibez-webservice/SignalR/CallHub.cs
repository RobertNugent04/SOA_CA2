using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SOA_CA2.SignalR
{
    /// <summary>
    /// SignalR hub for managing real-time call signaling for WebRTC.
    /// </summary>
    public class CallHub : Hub
    {
        /// <summary>
        /// Sends an offer to the receiver.
        /// </summary>
        public async Task SendOffer(string receiverId, object offer)
        {
            await Clients.User(receiverId).SendAsync("ReceiveOffer", offer);
        }

        /// <summary>
        /// Sends an answer back to the caller.
        /// </summary>
        public async Task SendAnswer(string callerId, object answer)
        {
            await Clients.User(callerId).SendAsync("ReceiveAnswer", answer);
        }

        /// <summary>
        /// Sends an ICE candidate to the specified user.
        /// </summary>
        public async Task SendIceCandidate(string userId, object candidate)
        {
            await Clients.User(userId).SendAsync("ReceiveIceCandidate", candidate);
        }

        /// <summary>
        /// Ends the call for the specified user.
        /// </summary>
        public async Task EndCall(string userId)
        {
            await Clients.User(userId).SendAsync("CallEnded");
        }
    }
}
