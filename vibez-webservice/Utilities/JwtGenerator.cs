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

        public JwtGenerator(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Key"];
        }

        public string GenerateToken(User user)
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
