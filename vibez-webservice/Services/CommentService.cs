using AutoMapper;
using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Comment;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Implements business logic for managing comments.
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentService"/> class.
        /// </summary>
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CommentDto>> GetCommentsForPostAsync(int postId)
        {
            try
            {
                _logger.LogInformation("Fetching comments for post ID: {PostId}", postId);
                IEnumerable<Comment> comments = await _unitOfWork.Comments.GetCommentsByPostIdAsync(postId);
                return comments.Select(c => _mapper.Map<CommentDto>(c));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching comments for post ID: {PostId}", postId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task CreateCommentAsync(int userId, CommentCreationDto dto)
        {
            try
            {
                _logger.LogInformation("Creating a comment for post ID: {PostId}", dto.PostId);

                Comment comment = new Comment
                {
                    UserId = userId,
                    PostId = dto.PostId,
                    Content = dto.Content,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Comments.AddCommentAsync(comment);
                await _unitOfWork.Comments.SaveChangesAsync();

                _logger.LogInformation("Comment created successfully for post ID: {PostId}", dto.PostId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating comment for post ID: {PostId}", dto.PostId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateCommentAsync(int userId, int commentId, CommentUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Updating comment ID: {CommentId}", commentId);
                Comment? comment = await _unitOfWork.Comments.GetCommentByIdAsync(commentId);

                if (comment == null || comment.UserId != userId)
                {
                    _logger.LogWarning("Unauthorized update attempt for comment ID: {CommentId}", commentId);
                    throw new UnauthorizedAccessException("You are not authorized to update this comment.");
                }

                if (!string.IsNullOrWhiteSpace(dto.Content))
                {
                    comment.Content = dto.Content;
                }

                await _unitOfWork.Comments.SaveChangesAsync();
                _logger.LogInformation("Comment updated successfully for comment ID: {CommentId}", commentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating comment ID: {CommentId}", commentId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeleteCommentAsync(int userId, int commentId)
        {
            try
            {
                _logger.LogInformation("Deleting comment ID: {CommentId}", commentId);
                Comment? comment = await _unitOfWork.Comments.GetCommentByIdAsync(commentId);

                if (comment == null || comment.UserId != userId)
                {
                    _logger.LogWarning("Unauthorized delete attempt for comment ID: {CommentId}", commentId);
                    throw new UnauthorizedAccessException("You are not authorized to delete this comment.");
                }

                await _unitOfWork.Comments.DeleteCommentAsync(commentId);
                await _unitOfWork.Comments.SaveChangesAsync();
                _logger.LogInformation("Comment deleted successfully for comment ID: {CommentId}", commentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment ID: {CommentId}", commentId);
                throw;
            }
        }
    }
}
