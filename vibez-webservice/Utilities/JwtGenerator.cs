using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SOA_CA2.Models;
using SOA_CA2.Interfaces;

namespace SOA_CA2.Utilities
{
    /// <summary>
    /// To generate JSON Web Tokens (JWT) for user authentication.
    /// </summary>
    public class JwtGenerator : IJwtGenerator
    {
        private readonly string _secretKey;
        private readonly ILogger<JwtGenerator> _logger;

        public JwtGenerator(IConfiguration configuration, ILogger<JwtGenerator> logger)
        {
            _secretKey = configuration["Jwt:Key"];
            _logger = logger;
        }

        public string GenerateToken(User user)
        {
            try
            {
                Claim[] claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                };

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: "vibez",
                    audience: "vibez",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds);

                string generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("JWT generated successfully for UserId {UserId}.", user.UserId);
                return generatedToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT for UserId {UserId}.", user.UserId);
                throw;
            }
        }
    }
}
