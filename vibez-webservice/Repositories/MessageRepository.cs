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
    /// Implements data access methods for the Message entity.
    /// </summary>
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MessageRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRepository"/> class.
        /// </summary>
        public MessageRepository(AppDbContext context, ILogger<MessageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AddMessageAsync(Message message)
        {
            try
            {
                _logger.LogInformation("Adding a new message.");
                await _context.Messages.AddAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new message.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Message>> GetUserMessagesAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching messages for user ID: {UserId}.", userId);
                return await _context.Messages
                    .Where(m => (m.SenderId == userId && !m.IsDeletedBySender) ||
                                (m.ReceiverId == userId && !m.IsDeletedByReceiver))
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching messages for user ID: {UserId}.", userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Message>> GetConversationMessagesAsync(int userId, int friendId)
        {
            try
            {
                _logger.LogInformation("Fetching conversation messages between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);
                return await _context.Messages
                    .Where(m => (m.SenderId == userId && m.ReceiverId == friendId && !m.IsDeletedBySender) ||
                                (m.SenderId == friendId && m.ReceiverId == userId && !m.IsDeletedByReceiver))
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching conversation messages between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateMessageAsync(Message message)
        {
            try
            {
                _logger.LogInformation("Updating message ID: {MessageId}.", message.MessageId);
                _context.Messages.Update(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating message ID: {MessageId}.", message.MessageId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Message?> GetMessageByIdAsync(int messageId)
        {
            try
            {
                _logger.LogInformation("Fetching message by ID: {MessageId}.", messageId);
                return await _context.Messages.FindAsync(messageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching message by ID: {MessageId}.", messageId);
                throw;
            }
        }

        /// <inheritdoc />
        public void RemoveMessage(Message message)
        {
            try
            {
                _logger.LogInformation("Removing message ID: {MessageId}.", message.MessageId);
                _context.Messages.Remove(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing message ID: {MessageId}.", message.MessageId);
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
