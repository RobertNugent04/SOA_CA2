using AutoMapper;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.User;
using SOA_CA2.Repositories;
using SOA_CA2.Utilities;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Service class implementing business logic for user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEmailService _emailService;
        private readonly IOtpCacheManager _otpCacheManager;
        private readonly PasswordHasher _passwordHasher;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IJwtGenerator jwtGenerator,
            IEmailService emailService,
            IOtpCacheManager otpCacheManager,
            PasswordHasher passwordHasher,
            ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
            _emailService = emailService;
            _otpCacheManager = otpCacheManager;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<bool> RegisterAsync(UserCreationDto dto)
        {
            try
            {
                _logger.LogInformation("Starting registration for email: {Email}", dto.Email);

                User? existingUser = await _unitOfWork.Users.FindByUsernameOrEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    if (!existingUser.IsActive)
                    {
                        if (!_otpCacheManager.HasValidOtp(existingUser.UserId))
                        {
                            _logger.LogInformation("Deleting unverified account for email: {Email}", dto.Email);
                            await _unitOfWork.Users.DeleteUserAsync(existingUser.UserId);
                            await _unitOfWork.Users.SaveChangesAsync();
                        }
                        else
                        {
                            throw new ArgumentException("An unverified account already exists. Please verify your account or wait for the OTP to expire.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Username or email already exists.");
                    }
                }

                // Proceed with new registration
                User user = _mapper.Map<User>(dto);
                user.PasswordHash = _passwordHasher.HashPassword(dto.Password);
                await _unitOfWork.Users.AddUserAsync(user);
                await _unitOfWork.Users.SaveChangesAsync();

                // Generate and send OTP
                string otp = OtpGenerator.GenerateOtp();
                _otpCacheManager.StoreOtp(user.UserId, otp, TimeSpan.FromMinutes(15));
                await _emailService.SendOtpEmailAsync(dto.Email, otp);

                _logger.LogInformation("Registration successful for email: {Email}", dto.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for email: {Email}", dto.Email);
                throw;
            }
        }


        /// <inheritdoc />
        public async Task VerifyOtpAsync(string email, string otp)
        {
            try
            {
                _logger.LogInformation("Verifying OTP for email: {Email}", email);

                User? user = await _unitOfWork.Users.FindByUsernameOrEmailAsync(email);
                if (user == null) throw new ArgumentException("User not found.");

                if (!_otpCacheManager.ValidateOtp(user.UserId, otp))
                {
                    throw new UnauthorizedAccessException("Invalid or expired OTP.");
                }

                user.IsActive = true;
                await _unitOfWork.Users.SaveChangesAsync();
                _otpCacheManager.InvalidateOtp(user.UserId);

                _logger.LogInformation("OTP verification successful for email: {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying OTP for email: {Email}", email);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<string?> LoginAsync(UserLoginDto dto)
        {
            try
            {
                _logger.LogInformation("Logging in user: {UsernameOrEmail}", dto.UserNameOrEmail);

                User? user = await _unitOfWork.Users.FindByUsernameOrEmailAsync(dto.UserNameOrEmail);
                if (user == null || !_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Invalid credentials.");
                }

                string token = _jwtGenerator.GenerateToken(user);
                _logger.LogInformation("Login successful for user: {UsernameOrEmail}", dto.UserNameOrEmail);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {UsernameOrEmail}", dto.UserNameOrEmail);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateUserProfileAsync(int userId, UserUpdateDto dto, string? profilePicturePath)
        {
            try
            {
                _logger.LogInformation("Updating profile for user ID: {UserId}", userId);

                User user = await _unitOfWork.Users.GetUserByIdAsync(userId)
                    ?? throw new ArgumentException("User not found.");

                if (!string.IsNullOrWhiteSpace(dto.FullName)) user.FullName = dto.FullName;
                if (!string.IsNullOrWhiteSpace(dto.Bio)) user.Bio = dto.Bio;
                if (!string.IsNullOrWhiteSpace(profilePicturePath)) user.ProfilePicturePath = profilePicturePath;
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.SaveChangesAsync();
                _logger.LogInformation("Profile updated successfully for user ID: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user ID: {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving user by ID: {UserId}", id);

                User? user = await _unitOfWork.Users.GetUserByIdAsync(id);
                return user != null ? _mapper.Map<UserDTO>(user) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID: {UserId}", id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task RequestPasswordResetAsync(string email)
        {
            try
            {
                _logger.LogInformation("Requesting password reset for email: {Email}", email);

                User? user = await _unitOfWork.Users.FindByUsernameOrEmailAsync(email);
                if (user == null)
                {
                    throw new ArgumentException("Email not found.");
                }

                string otp = OtpGenerator.GenerateOtp();
                _otpCacheManager.StoreOtp(user.UserId, otp, TimeSpan.FromMinutes(15));
                await _emailService.SendOtpEmailAsync(email, otp);

                _logger.LogInformation("Password reset OTP sent for email: {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting password reset for email: {Email}", email);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task VerifyOtpAndResetPasswordAsync(string email, string otp, string newPassword)
        {
            try
            {
                _logger.LogInformation("Verifying OTP for password reset: {Email}", email);

                User? user = await _unitOfWork.Users.FindByUsernameOrEmailAsync(email);
                if (user == null)
                {
                    throw new ArgumentException("Email not found.");
                }

                if (!_otpCacheManager.ValidateOtp(user.UserId, otp))
                {
                    throw new UnauthorizedAccessException("Invalid or expired OTP.");
                }

                user.PasswordHash = _passwordHasher.HashPassword(newPassword);
                await _unitOfWork.Users.SaveChangesAsync();
                _otpCacheManager.InvalidateOtp(user.UserId);

                _logger.LogInformation("Password reset successful for email: {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", email);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> SuggestUsernamesAsync(string fullName)
        {
            try
            {
                _logger.LogInformation("Generating username suggestions for full name: {FullName}", fullName);

                if (string.IsNullOrWhiteSpace(fullName))
                {
                    throw new ArgumentException("Full name must be provided.");
                }

                // Extract name parts and generate base username
                string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string baseUsername = string.Join("", nameParts).ToLower();

                List<string> suggestions = new List<string>();

                // Generate username suggestions
                for (int i = 1; i <= 5; i++)
                {
                    string suggestion = $"{baseUsername}{i}";
                    if (!await _unitOfWork.Users.UserNameExistsAsync(suggestion))
                    {
                        suggestions.Add(suggestion);
                    }
                }

                _logger.LogInformation("Generated {Count} username suggestions for full name: {FullName}", suggestions.Count, fullName);
                return suggestions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating username suggestions for full name: {FullName}", fullName);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserDTO>> SearchUsersAsync(string query)
        {
            try
            {
                _logger.LogInformation("Searching users with query: {Query}", query);

                if (string.IsNullOrWhiteSpace(query))
                {
                    _logger.LogWarning("Search query is empty or null.");
                    return Enumerable.Empty<UserDTO>();
                }

                // Perform user search
                IEnumerable<User> users = await _unitOfWork.Users.SearchUsersAsync(query);

                // Map users to DTOs
                IEnumerable<UserDTO> userDtos = users.Select(user => _mapper.Map<UserDTO>(user));

                _logger.LogInformation("Found {Count} users for query: {Query}", userDtos.Count(), query);
                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users with query: {Query}", query);
                throw;
            }
        }


        /// <inheritdoc />
        public async Task DeleteAccountAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Deleting account for user ID: {UserId}", userId);

                await _unitOfWork.Users.DeleteUserAsync(userId);
                await _unitOfWork.Users.SaveChangesAsync();

                _logger.LogInformation("Account deleted successfully for user ID: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account for user ID: {UserId}", userId);
                throw;
            }
        }
    }
}