﻿using Microsoft.AspNetCore.Mvc;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.Post;
using SOA_CA2.Middleware;
using System.Security.Claims;

namespace SOA_CA2.Controllers
{
    /// <summary>
    /// API controller for managing posts.
    /// </summary>
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostController"/> class.
        /// </summary>
        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all posts with pagination.
        /// </summary>
        /// <param name="pageNumber">The current page number (default: 1).</param>
        /// <param name="pageSize">The number of posts per page (default: 20).</param>
        /// <returns>A list of post DTOs.</returns>
        [HttpGet("activity-feed")]
        public async Task<IActionResult> GetActivityFeed(int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("Fetching activity feed for page {PageNumber}.", pageNumber);

                IEnumerable<PostDTO> posts = await _postService.GetAllPostsAsync(pageNumber, pageSize);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching activity feed.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all posts for the userId with pagination.
        /// </summary>
        [AuthorizeUser]
        [HttpGet("user-posts/{userId}")]
        public async Task<IActionResult> GetPosts(int userId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (userId <= 0)
                {
                    _logger.LogWarning("Invalid userId in status request.");
                    return BadRequest(new { Error = "Invalid userId." });
                }
                IEnumerable<PostDTO> posts = await _postService.GetPostsAsync(userId, pageNumber, pageSize);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching posts for user.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a single post by its ID.
        /// </summary>
        [AuthorizeUser]
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            try
            {
                PostDTO? post = await _postService.GetPostByIdAsync(postId);
                if (post == null)
                {
                    return NotFound(new { Error = "Post not found." });
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching post ID: {PostId}", postId);
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new post with an optional image.
        /// </summary>
        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] PostCreationDto dto)
        {
            try
            {
                // Validate content
                if (!string.IsNullOrWhiteSpace(dto.Content))
                {
                    dto.Content = dto.Content.Trim();
                    if (dto.Content.Length > 500)
                    {
                        return BadRequest(new { Error = "Post content cannot exceed 500 characters." });
                    }
                }

                int userId = GetUserIdFromToken();
                string? imagePath = SaveImage(Request.Form.Files["ImageUrl"], "posts-images");

                await _postService.CreatePostAsync(userId, dto, imagePath);
                return Ok(new { Message = "Post created successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid image file.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating post.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing post with optional content and image update.
        /// </summary>
        [AuthorizeUser]
        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromForm] PostUpdateDto dto)
        {
            try
            {
                // Validate content
                if (!string.IsNullOrWhiteSpace(dto.Content))
                {
                    dto.Content = dto.Content.Trim();
                    if (dto.Content.Length > 500)
                    {
                        return BadRequest(new { Error = "Post content cannot exceed 500 characters." });
                    }
                }

                int userId = GetUserIdFromToken();
                string? imagePath = SaveImage(Request.Form.Files["ImageUrl"], "posts-images");

                await _postService.UpdatePostAsync(userId, postId, dto, imagePath);
                return Ok(new { Message = "Post updated successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid image file.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating post.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a post.
        /// </summary>
        [AuthorizeUser]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                int userId = GetUserIdFromToken();
                await _postService.DeletePostAsync(userId, postId);
                return Ok(new { Message = "Post deleted successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post ID: {PostId}", postId);
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
        /// Method to save an image file to the server.
        /// </summary>
        private string? SaveImage(IFormFile? file, string folder)
        {
            if (file == null) return null;

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed.");
            }

            // Validate file size
            const long maxFileSize = 10 * 1024 * 1024; // 10MB
            if (file.Length > maxFileSize)
            {
                throw new ArgumentException("File size exceeds the maximum limit of 10MB.");
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return $"/{folder}/{uniqueFileName}";
        }
    }
}
