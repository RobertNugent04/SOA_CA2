using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SOA_CA2.Infrastructure;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;

namespace SOA_CA2.Repositories
{
    /// <summary>
    /// Implements data access methods for the Friendship entity.
    /// </summary>
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FriendshipRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipRepository"/> class.
        /// </summary>
        public FriendshipRepository(AppDbContext context, ILogger<FriendshipRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AddFriendRequestAsync(Friendship friendship)
        {
            try
            {
                _logger.LogInformation("Adding a friend request from user ID: {UserId} to user ID: {FriendId}.", friendship.UserId, friendship.FriendId);
                await _context.Friendships.AddAsync(friendship);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding friend request from user ID: {UserId} to user ID: {FriendId}.", friendship.UserId, friendship.FriendId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Friendship?> GetFriendshipByIdAsync(int friendshipId)
        {
            try
            {
                _logger.LogInformation("Fetching friendship by ID: {FriendshipId}.", friendshipId);
                return await _context.Friendships.FindAsync(friendshipId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching friendship by ID: {FriendshipId}.", friendshipId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateFriendshipAsync(Friendship friendship)
        {
            try
            {
                _logger.LogInformation("Updating friendship ID: {FriendshipId}.", friendship.FriendshipId);
                _context.Friendships.Update(friendship);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating friendship ID: {FriendshipId}.", friendship.FriendshipId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> FriendshipExistsAsync(int userId, int friendId)
        {
            try
            {
                _logger.LogInformation("Checking if friendship exists between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);
                // Check if a friendship exists between the two users.
                return await _context.Friendships.AnyAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                                                (f.UserId == friendId && f.FriendId == userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking friendship existence between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Friendship?> GetFriendshipBetweenUsersAsync(int userId, int friendId)
        {
            try
            {
                _logger.LogInformation("Fetching friendship between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);

                return await _context.Friendships
                    .FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                              (f.UserId == friendId && f.FriendId == userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching friendship between user ID: {UserId} and friend ID: {FriendId}.", userId, friendId);
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
