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

        public void StoreOtp(int userId, string otp, TimeSpan expiration)
        {
            _otpCache[userId] = (otp, DateTime.UtcNow.Add(expiration));
        }

        public bool ValidateOtp(int userId, string otp)
        {
            if (_otpCache.TryGetValue(userId, out var storedOtp))
            {
                if (storedOtp.Otp == otp && DateTime.UtcNow <= storedOtp.Expiration)
                {
                    return true;
                }
            }
            return false;
        }

        public void InvalidateOtp(int userId)
        {
            _otpCache.TryRemove(userId, out _);
        }
    }
}
