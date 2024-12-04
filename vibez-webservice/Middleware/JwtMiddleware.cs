using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SOA_CA2.Middleware
{
    /// <summary>
    /// Middleware to validate JWT tokens for protected routes.
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtMiddleware> _logger;

        /// <summary>
        /// Initializes the middleware with the request delegate, configuration, and logger.
        /// </summary>
        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Validates the JWT token and attaches the claims to the HttpContext.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    byte[] key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                    TokenValidationParameters parameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };

                    System.Security.Claims.ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, parameters, out _);

                    _logger.LogInformation("Token validated successfully. Attaching claims to HttpContext.");
                    foreach (System.Security.Claims.Claim claim in claimsPrincipal.Claims)
                    {
                        _logger.LogDebug("Claim Type: {Type}, Value: {Value}", claim.Type, claim.Value);
                    }

                    context.Items["User"] = claimsPrincipal;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Token validation failed.");
                }
            }
            else
            {
                _logger.LogInformation("No Authorization token found in the request.");
            }

            await _next(context);
        }
    }
}
