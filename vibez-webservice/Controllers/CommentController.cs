﻿using Microsoft.AspNetCore.Mvc;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.Comment;
using SOA_CA2.Middleware;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace SOA_CA2.Controllers
{
    /// <summary>
    /// API controller for managing comments on posts.
    /// </summary>
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentController"/> class.
        /// </summary>
        /// <param name="commentService">The service to manage comment operations.</param>
        /// <param name="logger">Logger for tracking operations.</param>
        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all comments for a given post ID.
        /// </summary>
        /// <param name="postId">The ID of the post to retrieve comments for.</param>
        /// <returns>A list of comments associated with the post.</returns>
        [AuthorizeUser]
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsForPost(int postId)
        {
            try
            {
                // Validate input
                if (postId <= 0)
                {
                    _logger.LogWarning("Invalid post ID provided: {PostId}", postId);
                    return BadRequest(new { Error = "Invalid post ID." });
                }

                _logger.LogInformation("Fetching comments for post ID: {PostId}", postId);
                IEnumerable<CommentDto> comments = await _commentService.GetCommentsForPostAsync(postId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching comments for post ID: {PostId}", postId);
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new comment on a post.
        /// </summary>
        /// <param name="dto">The data transfer object containing comment content and the post ID.</param>
        /// <returns>A success message if the comment is created successfully.</returns>
        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreationDto dto)
        {
            try
            {
                // Validate input
                if (dto.PostId <= 0)
                {
                    _logger.LogWarning("Invalid post ID provided: {PostId}", dto.PostId);
                    return BadRequest(new { Error = "Invalid post ID." });
                }

                if (string.IsNullOrWhiteSpace(dto.Content) || dto.Content.Length > 500)
                {
                    _logger.LogWarning("Invalid comment content.");
                    return BadRequest(new { Error = "Comment content must be between 1 and 500 characters." });
                }

                // Sanitize content
                dto.Content = SanitizeInput(dto.Content);

                int userId = GetUserIdFromToken();
                await _commentService.CreateCommentAsync(userId, dto);
                return Ok(new { Message = "Comment created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating comment for post ID: {PostId}", dto.PostId);
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment to update.</param>
        /// <param name="dto">The data transfer object containing updated comment content.</param>
        /// <returns>A success message if the comment is updated successfully.</returns>
        [AuthorizeUser]
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentUpdateDto dto)
        {
            try
            {
                // Validate input
                if (commentId <= 0)
                {
                    _logger.LogWarning("Invalid comment ID provided: {CommentId}", commentId);
                    return BadRequest(new { Error = "Invalid comment ID." });
                }

                if (string.IsNullOrWhiteSpace(dto.Content) || dto.Content.Length > 500)
                {
                    _logger.LogWarning("Invalid comment content.");
                    return BadRequest(new { Error = "Comment content must be between 1 and 500 characters." });
                }

                // Sanitize content
                dto.Content = SanitizeInput(dto.Content);

                int userId = GetUserIdFromToken();
                await _commentService.UpdateCommentAsync(userId, commentId, dto);
                return Ok(new { Message = "Comment updated successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized update attempt for comment ID: {CommentId}", commentId);
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating comment ID: {CommentId}", commentId);
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an existing comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment to delete.</param>
        /// <returns>A success message if the comment is deleted successfully.</returns>
        [AuthorizeUser]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                // Validate input
                if (commentId <= 0)
                {
                    _logger.LogWarning("Invalid comment ID provided: {CommentId}", commentId);
                    return BadRequest(new { Error = "Invalid comment ID." });
                }

                int userId = GetUserIdFromToken();
                await _commentService.DeleteCommentAsync(userId, commentId);
                return Ok(new { Message = "Comment deleted successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized delete attempt for comment ID: {CommentId}", commentId);
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment ID: {CommentId}", commentId);
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the user ID from the JWT token.
        /// </summary>
        /// <returns>The user ID extracted from the token.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authenticated.</exception>
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
        /// Sanitizes user input to prevent attacks like XSS or SQL injection.
        /// </summary>
        /// <param name="input">The user input to sanitize.</param>
        /// <returns>Sanitized input.</returns>
        private string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // Remove potentially dangerous
            return Regex.Replace(input, @"<.*?>", string.Empty).Trim();
        }
    }
}
