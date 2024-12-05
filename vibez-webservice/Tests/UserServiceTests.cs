using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.User;
using SOA_CA2.Services;
using SOA_CA2.Utilities;
using Xunit;

namespace SOA_CA2.Tests
{
    /// <summary>
    /// Unit tests for the UserService class.
    /// </summary>
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IOtpCacheManager> _otpCacheManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly PasswordHasher _passwordHasher;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            // Mock dependencies
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtGeneratorMock = new Mock<IJwtGenerator>();
            _emailServiceMock = new Mock<IEmailService>();
            _otpCacheManagerMock = new Mock<IOtpCacheManager>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _passwordHasher = new PasswordHasher(new Mock<ILogger<PasswordHasher>>().Object);

            // Initialize UserService with mocked dependencies
            _userService = new UserService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _jwtGeneratorMock.Object,
                _emailServiceMock.Object,
                _otpCacheManagerMock.Object,
                _passwordHasher,
                _loggerMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            UserLoginDto dto = new UserLoginDto { UserNameOrEmail = "test@example.com", Password = "password123" };
            User user = new User
            {
                UserId = 1,
                Email = dto.UserNameOrEmail,
                FullName = "Test User",
                UserName = "testuser123",
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                IsActive = true
            };

            _unitOfWorkMock.Setup(u => u.Users.FindByUsernameOrEmailAsync(dto.UserNameOrEmail)).ReturnsAsync(user);
            _jwtGeneratorMock.Setup(j => j.GenerateToken(user)).Returns("valid_token");

            // Act
            string? result = await _userService.LoginAsync(dto);

            // Assert
            Assert.Equal("valid_token", result);
            _jwtGeneratorMock.Verify(j => j.GenerateToken(user), Times.Once);
        }

        [Fact]
        public async Task SuggestUsernamesAsync_ShouldReturnUniqueSuggestions_WhenFullNameProvided()
        {
            // Arrange
            string fullName = "Test User";
            _unitOfWorkMock.Setup(u => u.Users.UserNameExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            IEnumerable<string> suggestions = await _userService.SuggestUsernamesAsync(fullName);

            // Assert
            Assert.NotEmpty(suggestions);
            Assert.Equal(5, suggestions.Count());
        }

        [Fact]
        public async Task SearchUsersAsync_ShouldReturnUsers_WhenQueryMatches()
        {
            // Arrange
            string query = "test";
            List<User> users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    FullName = "Test User 1",
                    UserName = "test1",
                    Email = "test1@example.com",
                    PasswordHash = _passwordHasher.HashPassword("password123"),
                    IsActive = true
                },
                new User
                {
                    UserId = 2,
                    FullName = "Test User 2",
                    UserName = "test2",
                    Email = "test2@example.com",
                    PasswordHash = _passwordHasher.HashPassword("password123"),
                    IsActive = true
                }
            };

            _unitOfWorkMock.Setup(u => u.Users.SearchUsersAsync(query)).ReturnsAsync(users);

            // Act
            IEnumerable<UserDTO> result = await _userService.SearchUsersAsync(query);

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}
