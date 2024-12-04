using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Post;
using SOA_CA2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SOA_CA2.Tests
{
    public class PostServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<PostService>> _loggerMock;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<PostService>>();
            _postService = new PostService(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllPostsAsync_ShouldReturnPosts_WhenPostsExist()
        {
            // Arrange
            int pageNumber = 1, pageSize = 10;
            List<Post> posts = new List<Post>
            {
                new Post
                {
                    PostId = 1,
                    UserId = 1,
                    Content = "Test Content 1",
                    ImageUrl = "/posts-images/test1.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Post
                {
                    PostId = 2,
                    UserId = 2,
                    Content = "Test Content 2",
                    ImageUrl = "/posts-images/test2.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _unitOfWorkMock.Setup(u => u.Posts.GetPaginatedPostsAsync(pageNumber, pageSize))
                .ReturnsAsync(posts);

            _mapperMock.Setup(m => m.Map<PostDTO>(It.IsAny<Post>()))
                .Returns((Post post) => new PostDTO
                {
                    PostId = post.PostId,
                    UserId = post.UserId,
                    Content = post.Content,
                    ImageUrl = post.ImageUrl,
                    CreatedAt = post.CreatedAt
                });

            // Act
            IEnumerable<PostDTO> result = await _postService.GetAllPostsAsync(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(posts.Count, result.Count());
        }

        [Fact]
        public async Task GetPostsAsync_ShouldReturnUserPosts_WhenPostsExistForUser()
        {
            // Arrange
            int userId = 1, pageNumber = 1, pageSize = 10;
            List<Post> posts = new List<Post>
            {
                new Post
                {
                    PostId = 1,
                    UserId = userId,
                    Content = "User Test Content",
                    ImageUrl = "/posts-images/user-test.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _unitOfWorkMock.Setup(u => u.Posts.GetAllPostsAsync(userId, pageNumber, pageSize))
                .ReturnsAsync(posts);

            // Act
            IEnumerable<PostDTO> result = await _postService.GetPostsAsync(userId, pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetPostByIdAsync_ShouldReturnPost_WhenPostExists()
        {
            // Arrange
            int postId = 1;
            Post post = new Post
            {
                PostId = postId,
                UserId = 1,
                Content = "Specific Post Content",
                ImageUrl = "/posts-images/specific.jpg",
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(u => u.Posts.GetPostByIdAsync(postId)).ReturnsAsync(post);

            // Act
            PostDTO? result = await _postService.GetPostByIdAsync(postId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postId, result.PostId);
        }

        [Fact]
        public async Task CreatePostAsync_ShouldAddPost_WhenValidDataProvided()
        {
            // Arrange
            int userId = 1;
            PostCreationDto dto = new PostCreationDto
            {
                Content = "New Post Content",
                ImageUrl = null
            };
            string? imagePath = "/posts-images/new-post.jpg";

            _unitOfWorkMock.Setup(u => u.Posts.AddPostAsync(It.IsAny<Post>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.Posts.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _postService.CreatePostAsync(userId, dto, imagePath);

            // Assert
            _unitOfWorkMock.Verify(u => u.Posts.AddPostAsync(It.IsAny<Post>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Posts.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdatePostAsync_ShouldUpdatePost_WhenAuthorizedUserUpdates()
        {
            // Arrange
            int userId = 1, postId = 1;
            PostUpdateDto dto = new PostUpdateDto
            {
                Content = "Updated Content",
                ImageUrl = null
            };
            string? imagePath = "/posts-images/updated.jpg";

            Post post = new Post
            {
                PostId = postId,
                UserId = userId,
                Content = "Original Content",
                ImageUrl = "/posts-images/original.jpg",
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(u => u.Posts.GetPostByIdAsync(postId)).ReturnsAsync(post);
            _unitOfWorkMock.Setup(u => u.Posts.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _postService.UpdatePostAsync(userId, postId, dto, imagePath);

            // Assert
            Assert.Equal(dto.Content, post.Content);
            Assert.Equal(imagePath, post.ImageUrl);
            _unitOfWorkMock.Verify(u => u.Posts.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeletePostAsync_ShouldRemovePost_WhenAuthorizedUserDeletes()
        {
            // Arrange
            int userId = 1, postId = 1;
            Post post = new Post
            {
                PostId = postId,
                UserId = userId,
                Content = "To Be Deleted",
                ImageUrl = "/posts-images/delete.jpg",
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(u => u.Posts.GetPostByIdAsync(postId)).ReturnsAsync(post);
            _unitOfWorkMock.Setup(u => u.Posts.DeletePostAsync(postId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Posts.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _postService.DeletePostAsync(userId, postId);

            // Assert
            _unitOfWorkMock.Verify(u => u.Posts.DeletePostAsync(postId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Posts.SaveChangesAsync(), Times.Once);
        }
    }
}
