using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Message;
using SOA_CA2.Models.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Implements business logic for managing messages.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MessageService> _logger;
        private readonly INotificationService _notificationService;

        public MessageService(IUnitOfWork unitOfWork, ILogger<MessageService> logger, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _notificationService = notificationService;
        }

        /// <inheritdoc />
        public async Task SendMessageAsync(int senderId, MessageCreationDto dto)
        {
            try
            {
                _logger.LogInformation("Sending message from user ID: {SenderId} to receiver ID: {ReceiverId}.", senderId, dto.ReceiverId);

                // Ensure sender and receiver are friends or pending
                bool isFriend = await _unitOfWork.Friendships.FriendshipExistsAsync(senderId, dto.ReceiverId);
                if (!isFriend)
                {
                    throw new ArgumentException("Users are not friends or friendship request is not pending.");
                }

                Message message = new Message
                {
                    SenderId = senderId,
                    ReceiverId = dto.ReceiverId,
                    Content = dto.Content,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Messages.AddMessageAsync(message);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Message sent successfully.");

                // Notify the receiver
                await _notificationService.SendNotificationAsync(senderId, new NotificationCreationDto
                {
                    UserId = dto.ReceiverId,
                    Type = "Message",
                    ReferenceId = message.MessageId,
                    Message = "sent you a message."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message from user ID: {SenderId} to receiver ID: {ReceiverId}.", senderId, dto.ReceiverId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MessageDto>> GetUserMessagesAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching all messages for user ID: {UserId}.", userId);

                IEnumerable<Message> messages = await _unitOfWork.Messages.GetUserMessagesAsync(userId);

                return messages.Select(m => new MessageDto
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    IsDeletedBySender = m.IsDeletedBySender,
                    IsDeletedByReceiver = m.IsDeletedByReceiver,
                    CreatedAt = m.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching messages for user ID: {UserId}.", userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MessageDto>> GetConversationMessagesAsync(int userId, int friendId)
        {
            try
            {
                _logger.LogInformation("Fetching conversation messages between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);

                IEnumerable<Message> messages = await _unitOfWork.Messages.GetConversationMessagesAsync(userId, friendId);

                return messages.Select(m => new MessageDto
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    IsDeletedBySender = m.IsDeletedBySender,
                    IsDeletedByReceiver = m.IsDeletedByReceiver,
                    CreatedAt = m.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching conversation messages between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeleteMessageAsync(int userId, int messageId)
        {
            try
            {
                _logger.LogInformation("Deleting message ID: {MessageId} for user ID: {UserId}.", messageId, userId);

                // Fetch the message
                Message? message = await _unitOfWork.Messages.GetMessageByIdAsync(messageId);

                if (message == null)
                {
                    throw new ArgumentException("Message not found.");
                }

                // Determine who is deleting the message
                if (message.SenderId == userId)
                {
                    message.IsDeletedBySender = true;
                }
                else if (message.ReceiverId == userId)
                {
                    message.IsDeletedByReceiver = true;
                }
                else
                {
                    throw new UnauthorizedAccessException("User is not authorized to delete this message.");
                }

                // If both sender and receiver have deleted the message, remove it
                if (message.IsDeletedBySender && message.IsDeletedByReceiver)
                {
                    _unitOfWork.Messages.RemoveMessage(message);
                }
                else
                {
                    await _unitOfWork.Messages.UpdateMessageAsync(message);
                }

                // Save changes
                await _unitOfWork.Messages.SaveChangesAsync();

                _logger.LogInformation("Message deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message ID: {MessageId} for user ID: {UserId}.", messageId, userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MessageDto>> GetConversationByMessageAsync(int messageId, int userId)
        {
            try
            {
                // Fetch the message
                Message? message = await _unitOfWork.Messages.GetMessageByIdAsync(messageId);

                if (message == null)
                {
                    throw new ArgumentException("Message not found.");
                }

                // Ensure the user is either the sender or receiver of the message
                if (message.SenderId != userId && message.ReceiverId != userId)
                {
                    throw new UnauthorizedAccessException("User is not authorized to view this conversation.");
                }

                // Fetch the full conversation
                IEnumerable<Message> conversation = await _unitOfWork.Messages.GetConversationMessagesAsync(message.SenderId, message.ReceiverId);

                return conversation.Select(m => new MessageDto
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    IsDeletedBySender = m.IsDeletedBySender,
                    IsDeletedByReceiver = m.IsDeletedByReceiver,
                    CreatedAt = m.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching conversation for message ID: {MessageId}.", messageId);
                throw;
            }
        }
    }
}
