using System.Collections.Concurrent;
using SOA_CA2.Interfaces;

namespace SOA_CA2.Utilities
{
    /// <summary>
    /// In-memory OTP cache manager for storing and validating OTPs.
    /// </summary>
    public class OtpCacheManager : IOtpCacheManager
    {
        private readonly ConcurrentDictionary<int, (string Otp, DateTime Expiration)> _otpCache = new();

        /// <summary>
        /// Stores an OTP for a user with an expiration time.
        /// </summary>
        public void StoreOtp(int userId, string otp, TimeSpan expiration)
        {
            _otpCache[userId] = (otp, DateTime.UtcNow.Add(expiration));
            Console.WriteLine($"Stored OTP: {otp} for UserID: {userId} with expiration: {DateTime.UtcNow.Add(expiration)}");
        }

        /// <summary>
        /// Validates an OTP for a user by checking the stored value and expiration.
        /// </summary>
        public bool ValidateOtp(int userId, string otp)
        {
            if (_otpCache.TryGetValue(userId, out var storedOtp))
            {
                Console.WriteLine($"Validating OTP for UserID: {userId}, Input OTP: {otp}, Stored OTP: {storedOtp.Otp}, Expiration: {storedOtp.Expiration}");
                if (storedOtp.Otp.Equals(otp, StringComparison.OrdinalIgnoreCase) && DateTime.UtcNow <= storedOtp.Expiration)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Invalidates an OTP by removing it from the cache.
        /// </summary>
        public void InvalidateOtp(int userId)
        {
            _otpCache.TryRemove(userId, out _);
            Console.WriteLine($"Invalidated OTP for UserID: {userId}");
        }

        /// <summary>
        /// Checks if a user has a valid OTP in the cache.
        /// </summary>
        public bool HasValidOtp(int userId)
        {
            if (_otpCache.TryGetValue(userId, out var storedOtp))
            {
                Console.WriteLine($"Checking OTP for UserID: {userId}, Expiration: {storedOtp.Expiration}");
                return DateTime.UtcNow <= storedOtp.Expiration;
            }
            Console.WriteLine($"No OTP found for UserID: {userId}");
            return false;
        }
    }
}
