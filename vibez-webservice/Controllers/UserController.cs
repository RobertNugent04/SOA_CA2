using Microsoft.AspNetCore.Mvc;
using SOA_CA2.Interfaces;
using SOA_CA2.Models.DTOs.PasswordReset;
using SOA_CA2.Models.DTOs.OtpVerification;
using SOA_CA2.Models.DTOs.User;

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
        /// Updates the profile of an existing user.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, UserUpdateDto dto)
        {
            try
            {
                await _userService.UpdateUserProfileAsync(id, dto);
                return Ok(new { Message = "Profile updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a user's details by their ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserDTO? user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { Error = "User not found." });

            return Ok(user);
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
    }
}
