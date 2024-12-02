using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SOA_CA2.Utilities
{
    /// <summary>
    /// Provides methods to securely hash and verify passwords.
    /// </summary>
    public static class PasswordHasher
    {
        private const int Iterations = 10000;

        /// <summary>
        /// Hashes a plaintext password.
        /// </summary>
        public static string HashPassword(string password)
        {
            byte[] salt = GenerateSalt();
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: 256 / 8);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Verifies a password against its hash.
        /// </summary>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split('.');
            if (parts.Length != 2) return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hash = Convert.FromBase64String(parts[1]);

            byte[] hashToCompare = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: 256 / 8);

            return hash.SequenceEqual(hashToCompare);
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }
    }
}
