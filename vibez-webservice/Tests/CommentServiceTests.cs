using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using AutoMapper;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Comment;
using SOA_CA2.Services;
using SOA_CA2.Interfaces;

namespace SOA_CA2.Tests
{
    public class CommentServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CommentService>> _loggerMock;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CommentService>>();
            _commentService = new CommentService(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetCommentsForPostAsync_ShouldReturnComments_WhenPostHasComments()
        {
            // Arrange
            int postId = 1;
            List<Comment> comments = new List<Comment>
        {
            new Comment { CommentId = 1, Content = "Great post!", UserId = 2, PostId = postId },
            new Comment { CommentId = 2, Content = "Nice work!", UserId = 3, PostId = postId }
        };

            _unitOfWorkMock.Setup(u => u.Comments.GetCommentsByPostIdAsync(postId))
                .ReturnsAsync(comments);

            _mapperMock.Setup(m => m.Map<IEnumerable<CommentDto>>(comments))
                .Returns(new List<CommentDto>
                {
                new CommentDto { CommentId = 1, Content = "Great post!", UserId = 2, PostId = postId },
                new CommentDto { CommentId = 2, Content = "Nice work!", UserId = 3, PostId = postId }
                });

            // Act
            IEnumerable<CommentDto> result = await _commentService.GetCommentsForPostAsync(postId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateCommentAsync_ShouldAddComment_WhenDataIsValid()
        {
            // Arrange
            int userId = 1;
            CommentCreationDto dto = new CommentCreationDto { Content = "This is my comment!", PostId = 1 };

            Comment comment = new Comment
            {
                CommentId = 1,
                Content = dto.Content,
                UserId = userId,
                PostId = dto.PostId,
                CreatedAt = DateTime.UtcNow
            };

            _mapperMock.Setup(m => m.Map<Comment>(dto))
                .Returns(comment);

            _unitOfWorkMock.Setup(u => u.Comments.AddCommentAsync(comment)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Comments.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _commentService.CreateCommentAsync(userId, dto);

            // Assert
            _unitOfWorkMock.Verify(u => u.Comments.AddCommentAsync(It.IsAny<Comment>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Comments.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCommentAsync_ShouldUpdateComment_WhenAuthorized()
        {
            // Arrange
            int userId = 1;
            int commentId = 1;
            CommentUpdateDto dto = new CommentUpdateDto { Content = "Updated comment content!" };

            Comment existingComment = new Comment
            {
                CommentId = commentId,
                Content = "Original content",
                UserId = userId,
                PostId = 1
            };

            _unitOfWorkMock.Setup(u => u.Comments.GetCommentByIdAsync(commentId)).ReturnsAsync(existingComment);
            _unitOfWorkMock.Setup(u => u.Comments.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _commentService.UpdateCommentAsync(userId, commentId, dto);

            // Assert
            Assert.Equal(dto.Content, existingComment.Content);
            _unitOfWorkMock.Verify(u => u.Comments.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCommentAsync_ShouldRemoveComment_WhenAuthorized()
        {
            // Arrange
            int userId = 1;
            int commentId = 1;

            Comment existingComment = new Comment
            {
                CommentId = commentId,
                Content = "Some comment",
                UserId = userId,
                PostId = 1
            };

            _unitOfWorkMock.Setup(u => u.Comments.GetCommentByIdAsync(commentId)).ReturnsAsync(existingComment);
            _unitOfWorkMock.Setup(u => u.Comments.DeleteCommentAsync(commentId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Comments.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _commentService.DeleteCommentAsync(userId, commentId);

            // Assert
            _unitOfWorkMock.Verify(u => u.Comments.DeleteCommentAsync(commentId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Comments.SaveChangesAsync(), Times.Once);
        }
    }
}
