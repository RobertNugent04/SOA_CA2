﻿using Microsoft.AspNetCore.Mvc;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.Message;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using SOA_CA2.Middleware;
using System.Text.RegularExpressions;

namespace SOA_CA2.Controllers
{
    /// <summary>
    /// API controller for managing messages between users.
    /// </summary>
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageController"/> class.
        /// </summary>
        public MessageController(IMessageService messageService, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        /// <summary>
        /// Sends a message to a user.
        /// </summary>
        /// <param name="dto">The DTO containing the message details.</param>
        /// <returns>Returns a success or failure message.</returns>
        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageCreationDto dto)
        {
            try
            {
                // Input validation
                if (dto == null)
                {
                    _logger.LogWarning("Message creation DTO is null.");
                    return BadRequest(new { Error = "Message content cannot be empty." });
                }

                dto.Content = dto.Content?.Trim();
                if (string.IsNullOrWhiteSpace(dto.Content))
                {
                    _logger.LogWarning("Message content is empty.");
                    return BadRequest(new { Error = "Message content cannot be empty." });
                }

                if (dto.Content.Length > 1000)
                {
                    _logger.LogWarning("Message content exceeds the maximum length.");
                    return BadRequest(new { Error = "Message content cannot exceed 1000 characters." });
                }

                if (!IsValidMessageContent(dto.Content))
                {
                    _logger.LogWarning("Message content contains invalid characters.");
                    return BadRequest(new { Error = "Message content contains invalid characters." });
                }

                // Sanitize content
                dto.Content = SanitizeInput(dto.Content);

                int senderId = GetUserIdFromToken();
                await _messageService.SendMessageAsync(senderId, dto);
                return Ok(new { Message = "Message sent successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all messages for the authenticated user.
        /// </summary>
        /// <returns>Returns a list of messages.</returns>
        [AuthorizeUser]
        [HttpGet]
        public async Task<IActionResult> GetUserMessages()
        {
            try
            {
                int userId = GetUserIdFromToken();
                IEnumerable<MessageDto> messages = await _messageService.GetUserMessagesAsync(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user messages.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all messages in a conversation between the authenticated user and another user.
        /// </summary>
        /// <param name="friendId">The ID of the other user.</param>
        /// <returns>Returns a list of conversation messages.</returns>
        [AuthorizeUser]
        [HttpGet("conversation/{friendId}")]
        public async Task<IActionResult> GetConversationMessages(int friendId)
        {
            try
            {
                int userId = GetUserIdFromToken();
                IEnumerable<MessageDto> messages = await _messageService.GetConversationMessagesAsync(userId, friendId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving conversation messages.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a message for the authenticated user.
        /// </summary>
        /// <param name="messageId">The ID of the message to delete.</param>
        /// <returns>Returns a success or failure message.</returns>
        [AuthorizeUser]
        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            try
            {
                int userId = GetUserIdFromToken();
                await _messageService.DeleteMessageAsync(userId, messageId);
                return Ok(new { Message = "Message deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the conversation for a specific message.
        /// </summary>
        /// <param name="messageId">The ID of the message to retrieve the conversation for.</param>
        /// <returns>Returns a list of conversation messages.</returns>
        [AuthorizeUser]
        [HttpGet("{messageId}")]
        public async Task<IActionResult> GetConversationByMessage(int messageId)
        {
            try
            {
                int userId = GetUserIdFromToken();
                IEnumerable<MessageDto> conversation = await _messageService.GetConversationByMessageAsync(messageId, userId);
                return Ok(conversation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching conversation for message ID: {MessageId}", messageId);
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the user ID from the JWT token.
        /// </summary>
        private int GetUserIdFromToken()
        {
            ClaimsPrincipal? claimsPrincipal = HttpContext.Items["User"] as ClaimsPrincipal;
            if (claimsPrincipal == null || !int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            return userId;
        }

        /// <summary>
        /// Validates message content to allow only safe characters.
        /// </summary>
        private bool IsValidMessageContent(string content)
        {
            string pattern = @"^[\w\s.,!?@#%&*()\-+=:;/'""\[\]{}<>|\\]*$"; // Allow common alphanumeric and punctuation
            return Regex.IsMatch(content, pattern);
        }

        /// <summary>
        /// Sanitizes input by removing or encoding potentially malicious characters.
        /// </summary>
        private string SanitizeInput(string input)
        {
            return input.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&#39;");
        }
    }
}
