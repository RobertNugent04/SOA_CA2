using SOA_CA2.Models;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Generates JSON Web Tokens (JWT) for user authentication.
    /// </summary>
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
    }
}
