using SOA_CA2.Models.DTOs.User;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface defining business logic methods for User-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        Task<bool> RegisterAsync(UserCreationDto dto);

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        Task<string?> LoginAsync(UserLoginDto dto);

        /// <summary>
        /// Updates user profile information.
        /// </summary>
        Task UpdateUserProfileAsync(int userId, UserUpdateDto dto);

        /// <summary>
        /// Retrieves user details by their unique identifier.
        /// </summary>
        Task<UserDTO?> GetUserByIdAsync(int id);

        /// <summary>
        /// Resets a user's password.
        /// </summary>
        Task ResetPasswordAsync(string email, string newPassword);
    }
}
