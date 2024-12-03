using AutoMapper;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.User;
using SOA_CA2.Utilities;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Service class implementing business logic for user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IJwtGenerator jwtGenerator,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
            _emailService = emailService;
        }

        /// <inheritdoc />
        public async Task<bool> RegisterAsync(UserCreationDto dto)
        {
            // Check if email or username exists.
            if (await _userRepository.UserNameExistsAsync(dto.UserName) ||
                await _userRepository.EmailExistsAsync(dto.Email))
            {
                throw new ArgumentException("Username or email already exists.");
            }

            // Map DTO to User entity.
            User user = _mapper.Map<User>(dto);

            // Hash the password.
            user.PasswordHash = PasswordHasher.HashPassword(dto.Password);

            // Add the user to the database.
            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            // Send OTP to verify email.
            string otp = OtpGenerator.GenerateOtp();
            await _emailService.SendOtpEmailAsync(dto.Email, otp);

            return true;
        }

        /// <inheritdoc />
        public async Task<string?> LoginAsync(UserLoginDto dto)
        {
            // Find user by username or email.
            User? user = await _userRepository.FindByUsernameOrEmailAsync(dto.UserNameOrEmail);
            if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            // Generate JWT token.
            return _jwtGenerator.GenerateToken(user);
        }

        /// <inheritdoc />
        public async Task UpdateUserProfileAsync(int userId, UserUpdateDto dto)
        {
            // Find user by ID.
            User user = await _userRepository.GetUserByIdAsync(userId)
                       ?? throw new ArgumentException("User not found.");

            // Update properties.
            if (!string.IsNullOrWhiteSpace(dto.FullName)) user.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Bio)) user.Bio = dto.Bio;
            if (!string.IsNullOrWhiteSpace(dto.ProfilePictureUrl)) user.ProfilePictureUrl = dto.ProfilePictureUrl;

            // Save changes.
            await _userRepository.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            // Retrieve user and map to DTO.
            User? user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? _mapper.Map<UserDTO>(user) : null;
        }

        /// <inheritdoc />
        public async Task ResetPasswordAsync(string email, string newPassword)
        {
            // Find user by email.
            User? user = await _userRepository.FindByUsernameOrEmailAsync(email);
            if (user == null) throw new ArgumentException("Email not found.");

            // Update password.
            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            await _userRepository.SaveChangesAsync();
        }
    }
}