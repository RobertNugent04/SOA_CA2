using SOA_CA2.Models;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface defining data access methods for the User entity.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Finds a user by their username or email.
        /// </summary>
        Task<User?> FindByUsernameOrEmailAsync(string usernameOrEmail);

        /// <summary>
        /// Checks if a username already exists in the system.
        /// </summary>
        Task<bool> UserNameExistsAsync(string username);

        /// <summary>
        /// Checks if an email is already registered in the system.
        /// </summary>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        Task AddUserAsync(User user);

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        Task SaveChangesAsync();
    }
}
