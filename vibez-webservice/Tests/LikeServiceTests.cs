using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using SOA_CA2.Services;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Like;
using SOA_CA2.Models.DTOs.Notification;
using System.Threading.Tasks;

namespace SOA_CA2.Tests
{
    public class LikeServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<LikeService>> _loggerMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly LikeService _likeService;

        public LikeServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<LikeService>>();
            _notificationServiceMock = new Mock<INotificationService>();
            _likeService = new LikeService(_unitOfWorkMock.Object, _loggerMock.Object, _notificationServiceMock.Object);
        }

        [Fact]
        public async Task AddLikeAsync_ShouldAddLikeAndSendNotification_WhenDoesNotExistAndNotOwner()
        {
            // Arrange
            int userId = 1;
            int postOwnerId = 2;
            LikeCreationDto dto = new LikeCreationDto { PostId = 1 };

            Post post = new Post 
            {
                PostId = dto.PostId,
                UserId = postOwnerId,
                Content = "Original Content",
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(u => u.Likes.LikeExistsAsync(userId, dto.PostId)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Posts.GetPostByIdAsync(dto.PostId)).ReturnsAsync(post);
            _unitOfWorkMock.Setup(u => u.Likes.AddLikeAsync(It.IsAny<Like>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Likes.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _likeService.AddLikeAsync(userId, dto);

            // Assert
            _unitOfWorkMock.Verify(u => u.Likes.AddLikeAsync(It.IsAny<Like>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Likes.SaveChangesAsync(), Times.Once);
            _notificationServiceMock.Verify(n => n.SendNotificationAsync(userId, It.Is<NotificationCreationDto>(n =>
                n.UserId == postOwnerId &&
                n.Type == "Post" &&
                n.ReferenceId == dto.PostId &&
                n.Message == "liked your post."
            )), Times.Once);
        }

        [Fact]
        public async Task AddLikeAsync_ShouldAddLikeWithoutNotification_WhenUserIsPostOwner()
        {
            // Arrange
            int userId = 1; // User liking their own post
            LikeCreationDto dto = new LikeCreationDto { PostId = 1 };

            Post post = new Post
            {
                PostId = dto.PostId,
                UserId = userId,
                Content = "Original Content",
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(u => u.Likes.LikeExistsAsync(userId, dto.PostId)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Posts.GetPostByIdAsync(dto.PostId)).ReturnsAsync(post);
            _unitOfWorkMock.Setup(u => u.Likes.AddLikeAsync(It.IsAny<Like>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Likes.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _likeService.AddLikeAsync(userId, dto);

            // Assert
            _unitOfWorkMock.Verify(u => u.Likes.AddLikeAsync(It.IsAny<Like>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Likes.SaveChangesAsync(), Times.Once);
            _notificationServiceMock.Verify(n => n.SendNotificationAsync(It.IsAny<int>(), It.IsAny<NotificationCreationDto>()), Times.Never);
        }

        [Fact]
        public async Task AddLikeAsync_ShouldThrowArgumentException_WhenAlreadyExists()
        {
            // Arrange
            int userId = 1;
            LikeCreationDto dto = new LikeCreationDto { PostId = 1 };

            _unitOfWorkMock.Setup(u => u.Likes.LikeExistsAsync(userId, dto.PostId)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _likeService.AddLikeAsync(userId, dto));
            _unitOfWorkMock.Verify(u => u.Likes.AddLikeAsync(It.IsAny<Like>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Likes.SaveChangesAsync(), Times.Never);
            _notificationServiceMock.Verify(n => n.SendNotificationAsync(It.IsAny<int>(), It.IsAny<NotificationCreationDto>()), Times.Never);
        }

        [Fact]
        public async Task RemoveLikeAsync_ShouldRemoveLike_WhenExists()
        {
            // Arrange
            int userId = 1;
            int postId = 1;

            _unitOfWorkMock.Setup(u => u.Likes.LikeExistsAsync(userId, postId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Likes.RemoveLikeAsync(userId, postId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Likes.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _likeService.RemoveLikeAsync(userId, postId);

            // Assert
            _unitOfWorkMock.Verify(u => u.Likes.RemoveLikeAsync(userId, postId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Likes.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoveLikeAsync_ShouldThrowArgumentException_WhenNotExists()
        {
            // Arrange
            int userId = 1;
            int postId = 1;

            _unitOfWorkMock.Setup(u => u.Likes.LikeExistsAsync(userId, postId)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _likeService.RemoveLikeAsync(userId, postId));
            _unitOfWorkMock.Verify(u => u.Likes.RemoveLikeAsync(userId, postId), Times.Never);
            _unitOfWorkMock.Verify(u => u.Likes.SaveChangesAsync(), Times.Never);
        }
    }
}