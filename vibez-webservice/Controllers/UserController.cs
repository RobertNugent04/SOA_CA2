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

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreationDto dto)
        {
            try
            {
                await _userService.RegisterAsync(dto);
                return Ok(new { Message = "User registered successfully. Check your email for OTP." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
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
                await _userService.VerifyOtpAsync(dto.Email, dto.Otp);
                return Ok(new { Message = "OTP verified successfully. Your account is now active." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
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
                string? token = await _userService.LoginAsync(dto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Updates the profile of the authenticated user, including profile picture upload with validation.
        /// </summary>
        /// <param name="dto">The user profile update data transfer object.</param>
        /// <returns>An IActionResult indicating success or failure.</returns>
        [AuthorizeUser]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UserUpdateDto dto)
        {
            try
            {
                // Retrieve the authenticated user's ID from the JWT claims
                ClaimsPrincipal? claimsPrincipal = HttpContext.Items["User"] as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    return Unauthorized(new { Error = "User is not authenticated." });
                }

                string? userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userIdFromToken))
                {
                    return Unauthorized(new { Error = "Invalid user" });
                }

                string? profilePicturePath = null;

                // Handle profile picture upload if present
                if (dto.ProfilePicture != null)
                {
                    // Validate the file type
                    string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    string fileExtension = Path.GetExtension(dto.ProfilePicture.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest(new { Error = "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed." });
                    }

                    // Validate the file size (max 5MB)
                    const long maxFileSize = 5 * 1024 * 1024; // 5MB in bytes
                    if (dto.ProfilePicture.Length > maxFileSize)
                    {
                        return BadRequest(new { Error = "File size exceeds the maximum limit of 5MB." });
                    }

                    // Save the file to the server
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile-pictures");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = $"{Guid.NewGuid()}_{dto.ProfilePicture.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Write the file to disk
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ProfilePicture.CopyToAsync(fileStream);
                    }

                    profilePicturePath = $"/profile-pictures/{uniqueFileName}";
                }

                // Update the user's profile
                await _userService.UpdateUserProfileAsync(userIdFromToken, dto, profilePicturePath);

                return Ok(new { Message = "Profile updated successfully." });
            }
            catch (ArgumentException ex)
            {
                // Handle errors related to invalid input or user not found
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle any unexpected server errors
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the profile of the authenticated user.
        /// </summary>
        [AuthorizeUser]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            // Retrieve the authenticated user's ID from the JWT claims
            ClaimsPrincipal? claimsPrincipal = HttpContext.Items["User"] as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return Unauthorized(new { Error = "User is not authenticated." });
            }

            string? userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userIdFromToken))
            {
                return Unauthorized(new { Error = "Invalid user." });
            }

            UserDTO? user = await _userService.GetUserByIdAsync(userIdFromToken);
            if (user == null)
                return NotFound(new { Error = "User not found." });

            return Ok(user);
        }

        /// <summary>
        /// Deletes the account of the authenticated user.
        /// </summary>
        [AuthorizeUser]
        [HttpDelete("account")]
        public async Task<IActionResult> DeleteAccount()
        {
            // Retrieve the authenticated user's ID from the JWT claims
            ClaimsPrincipal? claimsPrincipal = HttpContext.Items["User"] as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return Unauthorized(new { Error = "User is not authenticated." });
            }

            string? userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userIdFromToken))
            {
                return Unauthorized(new { Error = "Invalid user" });
            }

            await _userService.DeleteAccountAsync(userIdFromToken);
            return Ok(new { Message = "Account deleted successfully." });
        }

        /// <summary>
        /// Requests a password reset for a user.
        /// </summary>
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto request)
        {
            await _userService.RequestPasswordResetAsync(request.Email);
            return Ok(new { Message = "OTP sent to email." });
        }

        /// <summary>
        /// Resets a user's password using an OTP.
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto request)
        {
            await _userService.VerifyOtpAndResetPasswordAsync(request.Email, request.Otp, request.NewPassword);
            return Ok(new { Message = "Password reset successful." });
        }

        /// <summary>
        /// Suggests unique usernames based on the user's full name.
        /// </summary>
        [HttpGet("suggest-usernames")]
        public async Task<IActionResult> SuggestUsernames([FromQuery] string fullName)
        {
            try
            {
                IEnumerable<string> suggestions = await _userService.SuggestUsernamesAsync(fullName);
                return Ok(suggestions);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Searches for users by username or name.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            IEnumerable<UserDTO> users = await _userService.SearchUsersAsync(query);
            return Ok(users);
        }
    }
}
