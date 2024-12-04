using SOA_CA2.Models.DTOs.Message;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for managing business logic related to messages.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Sends a message from one user to another.
        /// </summary>
        Task SendMessageAsync(int senderId, MessageCreationDto dto);

        /// <summary>
        /// Retrieves all messages for a specific user.
        /// </summary>
        Task<IEnumerable<MessageDto>> GetUserMessagesAsync(int userId);

        /// <summary>
        /// Retrieves all messages in a conversation between two users.
        /// </summary>
        Task<IEnumerable<MessageDto>> GetConversationMessagesAsync(int userId, int friendId);

        /// <summary>
        /// Deletes a message for the user.
        /// </summary>
        Task DeleteMessageAsync(int userId, int messageId);
    }
}
