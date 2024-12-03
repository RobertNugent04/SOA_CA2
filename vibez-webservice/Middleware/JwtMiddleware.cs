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

        /// <summary>
        /// Initializes the middleware with the request delegate and configuration.
        /// </summary>
        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
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
                    // Log claims for debugging
                    foreach (var claim in claimsPrincipal.Claims)
                    {
                        Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                    }
                    context.Items["User"] = claimsPrincipal;
                }
                catch
                {
                    // Token validation failed, do nothing
                }
            }

            await _next(context);
        }
    }
}
