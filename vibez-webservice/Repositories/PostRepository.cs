using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SOA_CA2.Infrastructure;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;

namespace SOA_CA2.Repositories
{
    /// <summary>
    /// Implements data access methods for the Post entity.
    /// </summary>
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PostRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">Logger for debugging and tracking operations.</param>
        public PostRepository(AppDbContext context, ILogger<PostRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Post>> GetPaginatedPostsAsync(int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Fetching posts for page {PageNumber} with page size {PageSize}.", pageNumber, pageSize);

                List<Post> posts = await _context.Posts
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .ToListAsync();

                _logger.LogInformation("Fetched {Count} posts for page {PageNumber}.", posts.Count, pageNumber);
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching paginated posts.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Post>> GetAllPostsAsync(int userId, int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Fetching posts for user ID: {UserId}, Page: {Page}, Size: {Size}", userId, pageNumber, pageSize);
                return await _context.Posts
                    .Where(p => p.UserId == userId)
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching posts for user ID: {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Post?> GetPostByIdAsync(int postId)
        {
            try
            {
                _logger.LogInformation("Fetching post by ID: {PostId}", postId);
                return await _context.Posts.Where(p => p.PostId == postId).Include(p => p.User).Include(p => p.Likes).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching post by ID: {PostId}", postId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task AddPostAsync(Post post)
        {
            try
            {
                _logger.LogInformation("Adding a new post for user ID: {UserId}", post.UserId);
                await _context.Posts.AddAsync(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new post for user ID: {UserId}", post.UserId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdatePostAsync(Post post)
        {
            try
            {
                _logger.LogInformation("Updating post ID: {PostId}", post.PostId);
                _context.Posts.Update(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating post ID: {PostId}", post.PostId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeletePostAsync(int postId)
        {
            try
            {
                _logger.LogInformation("Deleting post ID: {PostId}", postId);
                Post? post = await GetPostByIdAsync(postId);
                if (post == null)
                {
                    throw new ArgumentException($"Post with ID {postId} not found.");
                }
                _context.Posts.Remove(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post ID: {PostId}", postId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> PostExistsAsync(int postId)
        {
            try
            {
                return await _context.Posts.AnyAsync(p => p.PostId == postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of post ID: {PostId}", postId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Post>> GetAllPostsByUserIdAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching all posts for user ID: {UserId}.", userId);
                return await _context.Posts
                    .Where(p => p.UserId == userId)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all posts for user ID: {UserId}.", userId);
                throw;
            }
        }


        /// <inheritdoc />
        public async Task SaveChangesAsync()
        {
            try
            {
                _logger.LogInformation("Saving changes to the database.");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to the database.");
                throw;
            }
        }
    }
}
