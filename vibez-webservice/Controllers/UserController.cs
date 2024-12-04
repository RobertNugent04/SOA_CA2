using Microsoft.AspNetCore.Mvc;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.PasswordReset;
using SOA_CA2.Models.DTOs.OtpVerification;
using SOA_CA2.Models.DTOs.User;
using SOA_CA2.Middleware;
using System.Security.Claims;

namespace SOA_CA2.Controllers
{
    /// <summary>
    /// API controller for managing user-related operations.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreationDto dto)
        {
            try
            {
                _logger.LogInformation("Registering a new user with email: {Email}", dto.Email);
                await _userService.RegisterAsync(dto);
                _logger.LogInformation("User registered successfully.");
                return Ok(new { Message = "User registered successfully. Check your email for OTP." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Failed to register user: {Email}", dto.Email);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during user registration.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Verifies the OTP sent to the user's email during registration.
        /// </summary>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationDto dto)
        {
            try
            {
                _logger.LogInformation("Verifying OTP for email: {Email}", dto.Email);
                await _userService.VerifyOtpAsync(dto.Email, dto.Otp);
                _logger.LogInformation("OTP verified successfully.");
                return Ok(new { Message = "OTP verified successfully. Your account is now active." });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Invalid OTP for email: {Email}", dto.Email);
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during OTP verification.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Logs in a user and returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            try
            {
                _logger.LogInformation("Logging in user with username/email: {UserNameOrEmail}", dto.UserNameOrEmail);
                string? token = await _userService.LoginAsync(dto);
                _logger.LogInformation("User logged in successfully.");
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Invalid login attempt for username/email: {UserNameOrEmail}", dto.UserNameOrEmail);
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during login.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Updates the profile of the authenticated user, including profile picture upload with validation.
        /// </summary>
        [AuthorizeUser]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UserUpdateDto dto)
        {
            try
            {
                ClaimsPrincipal? claimsPrincipal = HttpContext.Items["User"] as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    _logger.LogWarning("Unauthorized profile update attempt.");
                    return Unauthorized(new { Error = "User is not authenticated." });
                }

                string? userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userIdFromToken))
                {
                    _logger.LogWarning("Invalid user ID in token during profile update.");
                    return Unauthorized(new { Error = "Invalid user." });
                }

                string? profilePicturePath = null;
                if (dto.ProfilePicture != null)
                {
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    string fileExtension = Path.GetExtension(dto.ProfilePicture.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        _logger.LogWarning("Invalid file type for profile picture.");
                        return BadRequest(new { Error = "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed." });
                    }

                    const long maxFileSize = 5 * 1024 * 1024;
                    if (dto.ProfilePicture.Length > maxFileSize)
                    {
                        _logger.LogWarning("File size exceeds the maximum limit.");
                        return BadRequest(new { Error = "File size exceeds the maximum limit of 5MB." });
                    }

                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile-pictures");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = $"{Guid.NewGuid()}_{dto.ProfilePicture.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ProfilePicture.CopyToAsync(fileStream);
                    }

                    profilePicturePath = $"/profile-pictures/{uniqueFileName}";
                }

                await _userService.UpdateUserProfileAsync(userIdFromToken, dto, profilePicturePath);
                _logger.LogInformation("Profile updated successfully for user ID: {UserId}", userIdFromToken);
                return Ok(new { Message = "Profile updated successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input during profile update.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during profile update.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Retrieves the profile of the authenticated user.
        /// </summary>
        [AuthorizeUser]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                ClaimsPrincipal? claimsPrincipal = HttpContext.Items["User"] as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    _logger.LogWarning("Unauthorized profile access attempt.");
                    return Unauthorized(new { Error = "User is not authenticated." });
                }

                string? userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userIdFromToken))
                {
                    _logger.LogWarning("Invalid user ID in token during profile retrieval.");
                    return Unauthorized(new { Error = "Invalid user." });
                }

                UserDTO? user = await _userService.GetUserByIdAsync(userIdFromToken);
                if (user == null)
                {
                    _logger.LogWarning("User not found for ID: {UserId}", userIdFromToken);
                    return NotFound(new { Error = "User not found." });
                }

                _logger.LogInformation("Profile retrieved successfully for user ID: {UserId}", userIdFromToken);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during profile retrieval.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Deletes the account of the authenticated user.
        /// </summary>
        [AuthorizeUser]
        [HttpDelete("account")]
        public async Task<IActionResult> DeleteAccount()
        {
            try
            {
                ClaimsPrincipal? claimsPrincipal = HttpContext.Items["User"] as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    _logger.LogWarning("Unauthorized account deletion attempt.");
                    return Unauthorized(new { Error = "User is not authenticated." });
                }

                string? userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userIdFromToken))
                {
                    _logger.LogWarning("Invalid user ID in token during account deletion.");
                    return Unauthorized(new { Error = "Invalid user." });
                }

                await _userService.DeleteAccountAsync(userIdFromToken);
                _logger.LogInformation("Account deleted successfully for user ID: {UserId}", userIdFromToken);
                return Ok(new { Message = "Account deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error during account deletion.");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during account deletion.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Requests a password reset for a user.
        /// </summary>
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto request)
        {
            try
            {
                _logger.LogInformation("Password reset request for email: {Email}", request.Email);
                await _userService.RequestPasswordResetAsync(request.Email);
                _logger.LogInformation("OTP sent for password reset to email: {Email}", request.Email);
                return Ok(new { Message = "OTP sent to email." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid email during password reset request: {Email}", request.Email);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during password reset request.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Resets a user's password using an OTP.
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto request)
        {
            try
            {
                _logger.LogInformation("Password reset attempt for email: {Email}", request.Email);
                await _userService.VerifyOtpAndResetPasswordAsync(request.Email, request.Otp, request.NewPassword);
                _logger.LogInformation("Password reset successfully for email: {Email}", request.Email);
                return Ok(new { Message = "Password reset successful." });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Invalid or expired OTP during password reset for email: {Email}", request.Email);
                return Unauthorized(new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid email during password reset: {Email}", request.Email);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during password reset.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Suggests unique usernames based on the user's full name.
        /// </summary>
        [HttpGet("suggest-usernames")]
        public async Task<IActionResult> SuggestUsernames([FromQuery] string fullName)
        {
            try
            {
                _logger.LogInformation("Generating username suggestions for full name: {FullName}", fullName);
                IEnumerable<string> suggestions = await _userService.SuggestUsernamesAsync(fullName);
                _logger.LogInformation("Generated username suggestions for full name: {FullName}", fullName);
                return Ok(suggestions);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error generating username suggestions for full name: {FullName}", fullName);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during username suggestion generation.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Searches for users by username or name.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            try
            {
                _logger.LogInformation("Searching for users with query: {Query}", query);
                IEnumerable<UserDTO> users = await _userService.SearchUsersAsync(query);
                _logger.LogInformation("Found {UserCount} users for query: {Query}", users.Count(), query);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during user search.");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }
    }
}
