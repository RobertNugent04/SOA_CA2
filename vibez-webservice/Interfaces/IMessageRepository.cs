using SOA_CA2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for managing data access operations for the Message entity.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Adds a new message to the database.
        /// </summary>
        Task AddMessageAsync(Message message);

        /// <summary>
        /// Retrieves all messages for a specific user.
        /// </summary>
        Task<IEnumerable<Message>> GetUserMessagesAsync(int userId);

        /// <summary>
        /// Retrieves all messages in a conversation between two users.
        /// </summary>
        Task<IEnumerable<Message>> GetConversationMessagesAsync(int userId, int friendId);

        /// <summary>
        /// Deletes a message by marking it as deleted for the sender or receiver.
        /// </summary>
        Task UpdateMessageAsync(Message message);

        /// <summary>
        /// Retrieves a message by its ID.
        /// </summary>
        /// <param name="messageId">The ID of the message to retrieve.</param>
        /// <returns>The message entity if found; otherwise, null.</returns>
        Task<Message?> GetMessageByIdAsync(int messageId);

        /// <summary>
        /// Removes a message from the database.
        /// </summary>
        /// <param name="message">The message entity to remove.</param>
        void RemoveMessage(Message message);


        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        Task SaveChangesAsync();
    }
}
