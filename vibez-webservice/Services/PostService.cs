using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Post;
using System;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Implements business logic for managing posts.
    /// </summary>
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <param name="logger">Logger for debugging and tracking operations.</param>
        public PostService(IUnitOfWork unitOfWork, ILogger<PostService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PostDTO>> GetPostsAsync(int userId, int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Fetching posts for user ID: {UserId}, Page: {Page}, Size: {Size}", userId, pageNumber, pageSize);
                IEnumerable<Post> posts = await _unitOfWork.Posts.GetAllPostsAsync(userId, pageNumber, pageSize);
                return posts.Select(post => new PostDTO
                {
                    PostId = post.PostId,
                    UserId = post.UserId,
                    Content = post.Content,
                    ImageUrl = post.ImageUrl,
                    CreatedAt = post.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching posts for user ID: {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<PostDTO?> GetPostByIdAsync(int postId)
        {
            try
            {
                _logger.LogInformation("Fetching post by ID: {PostId}", postId);
                Post? post = await _unitOfWork.Posts.GetPostByIdAsync(postId);
                if (post == null)
                {
                    _logger.LogWarning("Post with ID: {PostId} not found.", postId);
                    return null;
                }

                return new PostDTO
                {
                    PostId = post.PostId,
                    UserId = post.UserId,
                    Content = post.Content,
                    ImageUrl = post.ImageUrl,
                    CreatedAt = post.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching post by ID: {PostId}", postId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task CreatePostAsync(int userId, PostCreationDto dto, string? imagePath)
        {
            try
            {
                _logger.LogInformation("Creating a new post for user ID: {UserId}", userId);
                Post post = new Post
                {
                    UserId = userId,
                    Content = dto.Content,
                    ImageUrl = imagePath,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Posts.AddPostAsync(post);
                await _unitOfWork.Posts.SaveChangesAsync();

                _logger.LogInformation("Post created successfully for user ID: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating post for user ID: {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdatePostAsync(int userId, int postId, PostUpdateDto dto, string? imagePath)
        {
            try
            {
                _logger.LogInformation("Updating post ID: {PostId} for user ID: {UserId}", postId, userId);
                Post? post = await _unitOfWork.Posts.GetPostByIdAsync(postId);

                if (post == null || post.UserId != userId)
                {
                    _logger.LogWarning("Unauthorized update attempt or post not found for post ID: {PostId}", postId);
                    throw new UnauthorizedAccessException("You are not authorized to update this post.");
                }

                if (!string.IsNullOrWhiteSpace(dto.Content))
                {
                    post.Content = dto.Content;
                }
                if (!string.IsNullOrWhiteSpace(imagePath))
                {
                    post.ImageUrl = imagePath;
                }
                post.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Posts.SaveChangesAsync();
                _logger.LogInformation("Post updated successfully for post ID: {PostId}", postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating post ID: {PostId}", postId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeletePostAsync(int userId, int postId)
        {
            try
            {
                _logger.LogInformation("Deleting post ID: {PostId} for user ID: {UserId}", postId, userId);
                Post? post = await _unitOfWork.Posts.GetPostByIdAsync(postId);

                if (post == null || post.UserId != userId)
                {
                    _logger.LogWarning("Unauthorized delete attempt or post not found for post ID: {PostId}", postId);
                    throw new UnauthorizedAccessException("You are not authorized to delete this post.");
                }

                await _unitOfWork.Posts.DeletePostAsync(postId);
                await _unitOfWork.Posts.SaveChangesAsync();

                _logger.LogInformation("Post deleted successfully for post ID: {PostId}", postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post ID: {PostId}", postId);
                throw;
            }
        }
    }
}
