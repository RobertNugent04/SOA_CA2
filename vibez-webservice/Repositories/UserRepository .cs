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

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<User?> GetUserByIdAsync(int id) =>
            await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.Likes)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.UserId == id);

        /// <inheritdoc />
        public async Task<User?> FindByUsernameOrEmailAsync(string usernameOrEmail) =>
            await _context.Users.FirstOrDefaultAsync(u =>
                u.UserName == usernameOrEmail || u.Email == usernameOrEmail);

        /// <inheritdoc />
        public async Task<bool> UserNameExistsAsync(string username) =>
            await _context.Users.AnyAsync(u => u.UserName == username);

        /// <inheritdoc />
        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        /// <inheritdoc />
        public async Task AddUserAsync(User user) =>
            await _context.Users.AddAsync(user);

        /// <inheritdoc />
        public async Task<IEnumerable<User>> SearchUsersAsync(string query)
        {
            return await _context.Users
                .Where(u => EF.Functions.ILike(u.UserName, $"%{query}%") || EF.Functions.ILike(u.FullName, $"%{query}%"))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteUserAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {id} not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
