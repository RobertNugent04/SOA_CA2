using System;

namespace SOA_CA2.Utilities
{
    /// <summary>
    /// Utility class for generating OTPs (One-Time Passwords).
    /// </summary>
    public static class OtpGenerator
    {
        /// <summary>
        /// Generates a 6-digit OTP.
        /// </summary>
        /// <returns>A string containing the OTP.</returns>
        public static string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
