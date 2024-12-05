using Microsoft.AspNetCore.SignalR;
using SOA_CA2.Models.DTOs.Call;
using System.Threading.Tasks;

namespace SOA_CA2.SignalR
{
    /// <summary>
    /// SignalR hub for managing real-time call signaling.
    /// </summary>
    public class CallHub : Hub
    {
        /// <summary>
        /// Initiates a call to a user.
        /// </summary>
        public async Task InitiateCall(int receiverId, CallDto call)
        {
            // Notify the receiver about the incoming call
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveCall", call);
        }

        /// <summary>
        /// Sends WebRTC offer or possibly also  answer to the remote peer.
        /// </summary>
        public async Task SendSignal(int userId, object signalData)
        {
            // Forward the signal data to the specified user
            await Clients.User(userId.ToString()).SendAsync("ReceiveSignal", signalData);
        }

        /// <summary>
        /// Ends the call.
        /// </summary>
        public async Task EndCall(int callId)
        {
            // Notify that the call has ended
            await Clients.Group(callId.ToString()).SendAsync("CallEnded");
        }

        /// <summary>
        /// Joins the call for signaling.
        /// </summary>
        public async Task JoinCall(int callId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, callId.ToString());
        }

        /// <summary>
        /// Leaves the call.
        /// </summary>
        public async Task LeaveCall(int callId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, callId.ToString());
        }
    }
}
