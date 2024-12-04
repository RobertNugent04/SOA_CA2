using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.Notification;
using System.Collections.Generic;
using System.Threading.Tasks;
using SOA_CA2.Middleware;
using System.Security.Claims;

namespace SOA_CA2.Controllers
{
    /// <summary>
    /// Controller for managing user notifications.
    /// </summary>
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Fetches all notifications for the authenticated user.
        /// </summary>
        /// <returns>A list of notifications.</returns>
        [AuthorizeUser]
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            try
            {
                int userId = GetUserIdFromToken();
                _logger.LogInformation("Fetching notifications for user ID: {UserId}.", userId);

                IEnumerable<NotificationDto> notifications = await _notificationService.GetUserNotificationsAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notifications for the user.");
                return StatusCode(500, new { Error = "An error occurred while fetching notifications." });
            }
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to mark as read.</param>
        /// <returns>A status message indicating the result of the operation.</returns>
        [AuthorizeUser]
        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            try
            {
                int userId = GetUserIdFromToken();
                _logger.LogInformation("{UserId} Marking notification ID: {NotificationId} as read.", userId, notificationId);

                await _notificationService.MarkNotificationAsReadAsync(userId, notificationId);
                return Ok(new { Message = "Notification marked as read." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid notification ID: {NotificationId}.", notificationId);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification ID: {NotificationId} as read.", notificationId);
                return StatusCode(500, new { Error = "An error occurred while marking the notification as read." });
            }
        }

        /// <summary>
        /// Sends a notification.
        /// </summary>
        /// <param name="notificationDto">The notification details.</param>
        /// <returns>A status message indicating the result of the operation.</returns>
        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] NotificationCreationDto notificationDto)
        {
            try
            {
                int senderId = GetUserIdFromToken();
                _logger.LogInformation("Sending notification from user ID: {SenderId} to user ID: {UserId}.", senderId, notificationDto.UserId);

                await _notificationService.SendNotificationAsync(senderId, notificationDto);
                return Ok(new { Message = "Notification sent successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error in sending notification: {ErrorMessage}.", ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification.");
                return StatusCode(500, new { Error = "An error occurred while sending the notification." });
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
    }
}
