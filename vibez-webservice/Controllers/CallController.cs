using Microsoft.AspNetCore.Mvc;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.Call;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SOA_CA2.Middleware;
using Microsoft.Extensions.Logging;

namespace SOA_CA2.Controllers
{
    /// <summary>
    /// API controller for managing calls between users.
    /// </summary>
    [ApiController]
    [Route("api/calls")]
    public class CallController : ControllerBase
    {
        private readonly ICallService _callService;
        private readonly ILogger<CallController> _logger;

        public CallController(ICallService callService, ILogger<CallController> logger)
        {
            _callService = callService;
            _logger = logger;
        }

        /// <summary>
        /// Initiates a call to another user.
        /// </summary>
        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> InitiateCall([FromBody] CallCreationDto dto)
        {
            try
            {
                int callerId = GetUserIdFromToken();

                // Prevent users from calling themselves
                if (callerId == dto.ReceiverId)
                {
                    return BadRequest(new { Error = "Users cannot call themselves." });
                }

                CallDto call = await _callService.InitiateCallAsync(callerId, dto);
                return Ok(call);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request while initiating call.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access during call initiation.");
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating call.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Updates the status of a call (Accept, Reject, End).
        /// </summary>
        [AuthorizeUser]
        [HttpPut("{callId}/status")]
        public async Task<IActionResult> UpdateCallStatus(int callId, [FromBody] CallStatusUpdateDto dto)
        {
            try
            {
                int userId = GetUserIdFromToken();
                await _callService.UpdateCallStatusAsync(callId, userId, dto);
                return Ok(new { Message = "Call status updated successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request while updating call status.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access during call status update.");
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating call status.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all calls for the authenticated user.
        /// </summary>
        [AuthorizeUser]
        [HttpGet]
        public async Task<IActionResult> GetUserCalls()
        {
            try
            {
                int userId = GetUserIdFromToken();
                IEnumerable<CallDto> calls = await _callService.GetCallsForUserAsync(userId);
                return Ok(calls);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user calls.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the user ID from the JWT token.
        /// </summary>
        private int GetUserIdFromToken()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.Items["User"] as ClaimsPrincipal;
            if (claimsPrincipal == null || !int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            return userId;
        }
    }
}
