using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;

namespace SOA_CA2.Utilities
{
    /// <summary>
    /// Provides methods to securely hash and verify passwords.
    /// </summary>
    public class PasswordHasher
    {
        private const int Iterations = 10000;
        private readonly ILogger<PasswordHasher> _logger;

        public PasswordHasher(ILogger<PasswordHasher> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Hashes a plaintext password.
        /// </summary>
        public string HashPassword(string password)
        {
            try
            {
                byte[] salt = GenerateSalt();
                byte[] hash = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: Iterations,
                    numBytesRequested: 256 / 8);

                _logger.LogInformation("Password hashed successfully.");
                return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error hashing password.");
                throw;
            }
        }

        /// <summary>
        /// Verifies a password against its hash.
        /// </summary>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                string[] parts = hashedPassword.Split('.');
                if (parts.Length != 2)
                {
                    _logger.LogWarning("Invalid hashed password format.");
                    return false;
                }

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] hash = Convert.FromBase64String(parts[1]);

                byte[] hashToCompare = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: Iterations,
                    numBytesRequested: 256 / 8);

                bool isMatch = hash.SequenceEqual(hashToCompare);
                _logger.LogInformation("Password verification result: {Result}", isMatch);
                return isMatch;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying password.");
                throw;
            }
        }

        private static byte[] GenerateSalt()
        {
            try
            {
                byte[] salt = new byte[16];
                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt);
                return salt;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error generating salt.", ex);
            }
        }
    }
}
