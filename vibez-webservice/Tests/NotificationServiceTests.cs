using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Notification;
using SOA_CA2.Services;
using SOA_CA2.SignalR;
using Xunit;

namespace SOA_CA2.Tests
{
    public class NotificationServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHubContext<NotificationHub>> _hubContextMock;
        private readonly Mock<ILogger<NotificationService>> _loggerMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly Mock<IHubClients> _hubClientsMock;
        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _hubContextMock = new Mock<IHubContext<NotificationHub>>();
            _loggerMock = new Mock<ILogger<NotificationService>>();
            _clientProxyMock = new Mock<IClientProxy>();
            _hubClientsMock = new Mock<IHubClients>();

            _hubContextMock.Setup(h => h.Clients).Returns(_hubClientsMock.Object);

            _notificationService = new NotificationService(
                _unitOfWorkMock.Object,
                _hubContextMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldSendNotification_WhenValidDataProvided()
        {
            // Arrange
            int senderId = 1;
            User sender = new User { UserId = senderId, UserName = "User1", Email = "example@mail.com", FullName = "Test User", PasswordHash = "7834xb6348t7ynw578o" };
            NotificationCreationDto dto = new NotificationCreationDto
            {
                UserId = 2,
                Type = "FriendRequest",
                ReferenceId = 1,
                Message = "sent you a friend request."
            };

            Notification notification = new Notification
            {
                NotificationId = 1,
                UserId = dto.UserId,
                SenderId = senderId,
                Type = dto.Type,
                ReferenceId = dto.ReferenceId,
                Message = "User1 sent you a friend request.",
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(u => u.Users.GetUserByIdAsync(senderId)).ReturnsAsync(sender);
            _unitOfWorkMock.Setup(u => u.Notifications.AddNotificationAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Notifications.SaveChangesAsync()).Returns(Task.CompletedTask);

            _hubClientsMock.Setup(h => h.User(dto.UserId.ToString())).Returns(_clientProxyMock.Object);

            // Act
            await _notificationService.SendNotificationAsync(senderId, dto);

            // Assert
            _unitOfWorkMock.Verify(u => u.Notifications.AddNotificationAsync(It.Is<Notification>(n =>
                n.UserId == dto.UserId &&
                n.SenderId == senderId &&
                n.Type == dto.Type &&
                n.ReferenceId == dto.ReferenceId &&
                n.Message == notification.Message)), Times.Once);

            _unitOfWorkMock.Verify(u => u.Notifications.SaveChangesAsync(), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("ReceiveNotification", It.IsAny<object[]>(), default), Times.Once);
        }

        [Fact]
        public async Task GetUserNotificationsAsync_ShouldReturnNotifications_WhenUserHasNotifications()
        {
            // Arrange
            int userId = 1;
            List<Notification> notifications = new List<Notification>
            {
                new Notification
                {
                    NotificationId = 1,
                    UserId = userId,
                    SenderId = 2,
                    Type = "Comment",
                    ReferenceId = 101,
                    Message = "User2 commented on your post.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Notification
                {
                    NotificationId = 2,
                    UserId = userId,
                    SenderId = 3,
                    Type = "FriendRequest",
                    ReferenceId = 102,
                    Message = "User3 sent you a friend request.",
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _unitOfWorkMock.Setup(u => u.Notifications.GetUserNotificationsAsync(userId))
                .ReturnsAsync(notifications);

            // Act
            IEnumerable<NotificationDto> result = await _notificationService.GetUserNotificationsAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _unitOfWorkMock.Verify(u => u.Notifications.GetUserNotificationsAsync(userId), Times.Once);
        }

        [Fact]
        public async Task MarkNotificationAsReadAsync_ShouldMarkAsRead_WhenNotificationBelongsToUser()
        {
            // Arrange
            int userId = 1;
            int notificationId = 10;
            Notification notification = new Notification
            {
                NotificationId = notificationId,
                UserId = userId,
                Type = "test",
                Message = "test",
                IsRead = false
            };

            _unitOfWorkMock.Setup(u => u.Notifications.GetNotificationByIdAsync(notificationId))
                .ReturnsAsync(notification);
            _unitOfWorkMock.Setup(u => u.Notifications.UpdateNotificationAsync(notification))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Notifications.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _notificationService.MarkNotificationAsReadAsync(userId, notificationId);

            // Assert
            Assert.True(notification.IsRead);
            _unitOfWorkMock.Verify(u => u.Notifications.UpdateNotificationAsync(It.Is<Notification>(n => n.IsRead)), Times.Once);
            _unitOfWorkMock.Verify(u => u.Notifications.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task MarkNotificationAsReadAsync_ShouldThrowUnauthorizedAccess_WhenNotificationDoesNotBelongToUser()
        {
            // Arrange
            int userId = 1;
            int notificationId = 10;
            Notification notification = new Notification
            {
                NotificationId = notificationId,
                UserId = 2, // Different user
                Type = "test",
                Message = "test",
                IsRead = false
            };

            _unitOfWorkMock.Setup(u => u.Notifications.GetNotificationByIdAsync(notificationId))
                .ReturnsAsync(notification);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _notificationService.MarkNotificationAsReadAsync(userId, notificationId));

            _unitOfWorkMock.Verify(u => u.Notifications.UpdateNotificationAsync(It.IsAny<Notification>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Notifications.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task MarkNotificationAsReadAsync_ShouldThrowArgumentException_WhenNotificationDoesNotExist()
        {
            // Arrange
            int userId = 1;
            int notificationId = 10;

            _unitOfWorkMock.Setup(u => u.Notifications.GetNotificationByIdAsync(notificationId))
                .ReturnsAsync((Notification)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _notificationService.MarkNotificationAsReadAsync(userId, notificationId));

            _unitOfWorkMock.Verify(u => u.Notifications.UpdateNotificationAsync(It.IsAny<Notification>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Notifications.SaveChangesAsync(), Times.Never);
        }
    }
}
