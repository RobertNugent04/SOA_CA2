using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Friendship;
using SOA_CA2.Services;
using Xunit;

namespace SOA_CA2.Tests
{
    public class FriendshipServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<FriendshipService>> _loggerMock;
        private readonly FriendshipService _friendshipService;

        public FriendshipServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<FriendshipService>>();
            _friendshipService = new FriendshipService(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task SendFriendRequestAsync_ShouldSendRequest_WhenNoExistingFriendship()
        {
            // Arrange
            int userId = 1;
            int friendId = 2;
            FriendshipCreationDto dto = new FriendshipCreationDto { FriendId = friendId };

            _unitOfWorkMock.Setup(u => u.Friendships.FriendshipExistsAsync(userId, friendId))
                .ReturnsAsync(false);

            // Act
            await _friendshipService.SendFriendRequestAsync(userId, dto);

            // Assert
            _unitOfWorkMock.Verify(u => u.Friendships.AddFriendRequestAsync(It.IsAny<Friendship>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SendFriendRequestAsync_ShouldThrowException_WhenFriendshipExists()
        {
            // Arrange
            int userId = 1;
            int friendId = 2;
            FriendshipCreationDto dto = new FriendshipCreationDto { FriendId = friendId };

            _unitOfWorkMock.Setup(u => u.Friendships.FriendshipExistsAsync(userId, friendId))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _friendshipService.SendFriendRequestAsync(userId, dto));
            _unitOfWorkMock.Verify(u => u.Friendships.AddFriendRequestAsync(It.IsAny<Friendship>()), Times.Never);
        }

        [Fact]
        public async Task UpdateFriendshipStatusAsync_ShouldUpdateStatus_WhenFriendshipExists()
        {
            // Arrange
            int userId = 1;
            int friendId = 2;
            string status = "Accepted";
            Friendship friendship = new Friendship { UserId = friendId, FriendId = userId, Status = "Pending" };

            _unitOfWorkMock.Setup(u => u.Friendships.GetFriendshipBetweenUsersAsync(userId, friendId))
                .ReturnsAsync(friendship);

            // Act
            await _friendshipService.UpdateFriendshipStatusAsync(userId, friendId, status);

            // Assert
            _unitOfWorkMock.Verify(u => u.Friendships.UpdateFriendshipAsync(It.Is<Friendship>(f => f.Status == status)), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateFriendshipStatusAsync_ShouldThrowException_WhenFriendshipDoesNotExist()
        {
            // Arrange
            int userId = 1;
            int friendId = 2;
            string status = "Accepted";

            _unitOfWorkMock.Setup(u => u.Friendships.GetFriendshipBetweenUsersAsync(userId, friendId))
                .ReturnsAsync((Friendship)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _friendshipService.UpdateFriendshipStatusAsync(userId, friendId, status));
            _unitOfWorkMock.Verify(u => u.Friendships.UpdateFriendshipAsync(It.IsAny<Friendship>()), Times.Never);
        }

        [Fact]
        public async Task GetFriendshipStatusAsync_ShouldReturnStatus_WhenFriendshipExists()
        {
            // Arrange
            int userId = 1;
            int friendId = 2;
            Friendship friendship = new Friendship { UserId = userId, FriendId = friendId, Status = "Pending" };

            _unitOfWorkMock.Setup(u => u.Friendships.GetFriendshipBetweenUsersAsync(userId, friendId))
                .ReturnsAsync(friendship);

            // Act
            string? status = await _friendshipService.GetFriendshipStatusAsync(userId, friendId);

            // Assert
            Assert.Equal("Pending", status);
        }

        [Fact]
        public async Task GetFriendshipStatusAsync_ShouldReturnNull_WhenFriendshipDoesNotExist()
        {
            // Arrange
            int userId = 1;
            int friendId = 2;

            _unitOfWorkMock.Setup(u => u.Friendships.GetFriendshipBetweenUsersAsync(userId, friendId))
                .ReturnsAsync((Friendship)null);

            // Act
            string? status = await _friendshipService.GetFriendshipStatusAsync(userId, friendId);

            // Assert
            Assert.Null(status);
        }
    }
}