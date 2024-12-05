using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Message;
using SOA_CA2.Models.DTOs.Notification;
using SOA_CA2.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SOA_CA2.SignalR;

namespace SOA_CA2.Tests
{
    public class MessageServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<MessageService>> _loggerMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<IHubContext<MessageHub>> _messageHubMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly Mock<IHubClients> _hubClientsMock;
        private readonly MessageService _messageService;

        public MessageServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<MessageService>>();
            _notificationServiceMock = new Mock<INotificationService>();
            _messageHubMock = new Mock<IHubContext<MessageHub>>();
            _clientProxyMock = new Mock<IClientProxy>();
            _hubClientsMock = new Mock<IHubClients>();

            // Mock SignalR Client Communication
            _hubClientsMock.Setup(clients => clients.User(It.IsAny<string>())).Returns(_clientProxyMock.Object);
            _messageHubMock.Setup(hub => hub.Clients).Returns(_hubClientsMock.Object);

            _messageService = new MessageService(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _notificationServiceMock.Object,
                _messageHubMock.Object);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldSendMessageAndTriggerNotification_WhenUsersAreFriends()
        {
            // Arrange
            int senderId = 1;
            int receiverId = 2;
            MessageCreationDto messageDto = new MessageCreationDto
            {
                ReceiverId = receiverId,
                Content = "Hello"
            };

            _unitOfWorkMock.Setup(u => u.Friendships.FriendshipExistsAsync(senderId, receiverId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.Messages.AddMessageAsync(It.IsAny<Message>()))
                .Verifiable();

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _notificationServiceMock.Setup(n => n.SendNotificationAsync(senderId, It.IsAny<NotificationCreationDto>()))
                .Returns(Task.CompletedTask);

            // Act
            await _messageService.SendMessageAsync(senderId, messageDto);

            // Assert
            _unitOfWorkMock.Verify(u => u.Messages.AddMessageAsync(It.Is<Message>(m =>
                m.SenderId == senderId &&
                m.ReceiverId == receiverId &&
                m.Content == "Hello")), Times.Once);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            _notificationServiceMock.Verify(n => n.SendNotificationAsync(senderId, It.Is<NotificationCreationDto>(n =>
                n.UserId == receiverId &&
                n.Type == "Message" &&
                n.Message == "sent you a message.")), Times.Once);

            _hubClientsMock.Verify(clients => clients.User(receiverId.ToString()), Times.Once);
            _clientProxyMock.Verify(client => client.SendCoreAsync("ReceiveMessage",
                It.Is<object[]>(args => args.Length == 1 && ((MessageDto)args[0]).Content == "Hello"),
                default), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldThrowArgumentException_WhenUsersAreNotFriends()
        {
            // Arrange
            int senderId = 1;
            int receiverId = 2;
            MessageCreationDto messageDto = new MessageCreationDto
            {
                ReceiverId = receiverId,
                Content = "Hello"
            };

            _unitOfWorkMock.Setup(u => u.Friendships.FriendshipExistsAsync(senderId, receiverId))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _messageService.SendMessageAsync(senderId, messageDto));

            _notificationServiceMock.Verify(n => n.SendNotificationAsync(It.IsAny<int>(), It.IsAny<NotificationCreationDto>()), Times.Never);
            _hubClientsMock.Verify(clients => clients.User(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetUserMessagesAsync_ShouldReturnUserMessages()
        {
            // Arrange
            int userId = 1;
            List<Message> messages = new List<Message>
            {
                new Message { MessageId = 1, SenderId = userId, ReceiverId = 2, Content = "Hello" },
                new Message { MessageId = 2, SenderId = 3, ReceiverId = userId, Content = "Hi" }
            };

            _unitOfWorkMock.Setup(u => u.Messages.GetUserMessagesAsync(userId))
                .ReturnsAsync(messages);

            // Act
            IEnumerable<MessageDto> result = await _messageService.GetUserMessagesAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Hello", result.First().Content);
            Assert.Equal("Hi", result.Last().Content);
        }

        [Fact]
        public async Task GetConversationMessagesAsync_ShouldReturnConversationMessages()
        {
            // Arrange
            int userId = 1;
            int friendId = 2;
            List<Message> messages = new List<Message>
            {
                new Message { MessageId = 1, SenderId = userId, ReceiverId = friendId, Content = "Hello" },
                new Message { MessageId = 2, SenderId = friendId, ReceiverId = userId, Content = "Hi" }
            };

            _unitOfWorkMock.Setup(u => u.Messages.GetConversationMessagesAsync(userId, friendId))
                .ReturnsAsync(messages);

            // Act
            IEnumerable<MessageDto> result = await _messageService.GetConversationMessagesAsync(userId, friendId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Hello", result.First().Content);
            Assert.Equal("Hi", result.Last().Content);
        }
    }
}
