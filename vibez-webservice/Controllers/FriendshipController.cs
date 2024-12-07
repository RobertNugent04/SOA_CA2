using Microsoft.AspNetCore.Mvc;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.Friendship;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using SOA_CA2.Middleware;

namespace SOA_CA2.Controllers
{
    /// <summary>
    /// API controller for managing friendships, such as sending requests, accepting, rejecting, and viewing friendship statuses.
    /// </summary>
    [ApiController]
    [Route("api/friendships")]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;
        private readonly ILogger<FriendshipController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipController"/> class.
        /// </summary>
        public FriendshipController(IFriendshipService friendshipService, ILogger<FriendshipController> logger)
        {
            _friendshipService = friendshipService;
            _logger = logger;
        }

        /// <summary>
        /// Sends a friend request to another user.
        /// </summary>
        /// <param name="dto">The DTO containing the friend ID to whom the request will be sent.</param>
        /// <returns>Returns a success or failure message.</returns>
        [AuthorizeUser]
        [HttpPost("send-request")]
        public async Task<IActionResult> SendFriendRequest([FromBody] FriendshipCreationDto dto)
        {
            try
            {
                int userId = GetUserIdFromToken();

                // Validate input
                if (dto.FriendId <= 0)
                {
                    _logger.LogWarning("Invalid FriendId in friend request.");
                    return BadRequest(new { Error = "Invalid FriendId." });
                }

                if (dto.FriendId == userId)
                {
                    _logger.LogWarning("User attempted to send a friend request to themselves.");
                    return BadRequest(new { Error = "You cannot send a friend request to yourself." });
                }

                await _friendshipService.SendFriendRequestAsync(userId, dto);
                return Ok(new { Message = "Friend request sent successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Friend request failed validation.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending friend request.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Accepts a pending friend request.
        /// </summary>
        /// <param name="friendId">The friend ID whose request to accept.</param>
        /// <returns>Returns a success or failure message.</returns>
        [AuthorizeUser]
        [HttpPut("accept/{friendId}")]
        public async Task<IActionResult> AcceptFriendRequest(int friendId)
        {
            try
            {
                int userId = GetUserIdFromToken();

                if (friendId <= 0)
                {
                    _logger.LogWarning("Invalid FriendId in accept request.");
                    return BadRequest(new { Error = "Invalid FriendId." });
                }

                await _friendshipService.UpdateFriendshipStatusAsync(userId, friendId, "Accepted");
                return Ok(new { Message = "Friend request accepted." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error accepting friend request.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting friend request.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Rejects a pending friend request.
        /// </summary>
        /// <param name="friendId">The friend ID whose request to reject.</param>
        /// <returns>Returns a success or failure message.</returns>
        [AuthorizeUser]
        [HttpPut("reject/{friendId}")]
        public async Task<IActionResult> RejectFriendRequest(int friendId)
        {
            try
            {
                int userId = GetUserIdFromToken();

                if (friendId <= 0)
                {
                    _logger.LogWarning("Invalid FriendId in reject request.");
                    return BadRequest(new { Error = "Invalid FriendId." });
                }

                await _friendshipService.UpdateFriendshipStatusAsync(userId, friendId, "Rejected");
                return Ok(new { Message = "Friend request rejected." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error rejecting friend request.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting friend request.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }



        /// <summary>
        /// Retrieves the status of the friendship between the authenticated user and another user.
        /// </summary>
        /// <param name="friendId">The ID of the friend to check the status.</param>
        /// <returns>Returns the friendship status.</returns>
        [AuthorizeUser]
        [HttpGet("status/{friendId}")]
        public async Task<IActionResult> GetFriendshipStatus(int friendId)
        {
            try
            {
                int userId = GetUserIdFromToken();

                if (friendId <= 0)
                {
                    _logger.LogWarning("Invalid FriendId in status request.");
                    return BadRequest(new { Error = "Invalid FriendId." });
                }

                string? friendshipStatus = await _friendshipService.GetFriendshipStatusAsync(userId, friendId);
                return Ok(new { Status = friendshipStatus });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving friendship status.");
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
    }
}
