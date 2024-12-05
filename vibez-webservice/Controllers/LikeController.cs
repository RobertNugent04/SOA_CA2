using Microsoft.AspNetCore.Mvc;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.Like;
using SOA_CA2.Middleware;
using System.Security.Claims;

namespace SOA_CA2.Controllers
{
    /// <summary>
    /// API controller for managing likes on posts.
    /// </summary>
    [ApiController]
    [Route("api/likes")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly ILogger<LikeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeController"/> class.
        /// </summary>
        public LikeController(ILikeService likeService, ILogger<LikeController> logger)
        {
            _likeService = likeService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a like to a post.
        /// </summary>
        /// <param name="dto">The like creation DTO.</param>
        /// <returns>Status indicating success or failure.</returns>
        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] LikeCreationDto dto)
        {
            try
            {
                int userId = GetUserIdFromToken();
                await _likeService.AddLikeAsync(userId, dto);
                return Ok(new { Message = "Post liked successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Conflict in liking post.");
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding like.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Removes a like from a post.
        /// </summary>
        /// <param name="postId">The ID of the post to unlike.</param>
        /// <returns>Status indicating success or failure.</returns>
        [AuthorizeUser]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> RemoveLike(int postId)
        {
            try
            {
                int userId = GetUserIdFromToken();
                await _likeService.RemoveLikeAsync(userId, postId);
                return Ok(new { Message = "Like removed successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Conflict in unliking post.");
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing like.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the user ID from the JWT token.
        /// </summary>
        /// <returns>The user ID.</returns>
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
