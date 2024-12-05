using Microsoft.EntityFrameworkCore;
using SOA_CA2.Infrastructure;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;

namespace SOA_CA2.Repositories
{
    /// <summary>
    /// Implements data access methods for the User entity.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">Logger for logging and troubleshooting.</param>
        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user by ID: {Id}", id);
                return await _context.Users
                    .Include(u => u.Posts)
                    .Include(u => u.Likes)
                    .Include(u => u.Comments)
                    .FirstOrDefaultAsync(u => u.UserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by ID: {Id}", id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<User?> FindByUsernameOrEmailAsync(string usernameOrEmail)
        {
            try
            {
                _logger.LogInformation("Fetching user by username or email: {UsernameOrEmail}", usernameOrEmail);
                return await _context.Users.FirstOrDefaultAsync(u =>
                    u.UserName == usernameOrEmail || u.Email == usernameOrEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by username or email: {UsernameOrEmail}", usernameOrEmail);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> UserNameExistsAsync(string username)
        {
            try
            {
                _logger.LogInformation("Checking if username exists: {Username}", username);
                return await _context.Users.AnyAsync(u => u.UserName == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if username exists: {Username}", username);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                _logger.LogInformation("Checking if email exists: {Email}", email);
                return await _context.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email exists: {Email}", email);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task AddUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("Adding new user: {UserName}", user.UserName);
                await _context.Users.AddAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new user: {UserName}", user.UserName);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> SearchUsersAsync(string query)
        {
            try
            {
                _logger.LogInformation("Searching users with query: {Query}", query);
                return await _context.Users
                    .Where(u => EF.Functions.ILike(u.UserName, $"%{query}%") || EF.Functions.ILike(u.FullName, $"%{query}%"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users with query: {Query}", query);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeleteUserAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {Id}", id);
                User? user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {Id} not found", id);
                    throw new ArgumentException($"User with ID {id} not found.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User with ID {Id} deleted successfully", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {Id}", id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync()
        {
            try
            {
                _logger.LogInformation("Saving changes to the database");
                await _context.SaveChangesAsync();
                _logger.LogInformation("Database changes saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to the database");
                throw;
            }
        }
    }
}
