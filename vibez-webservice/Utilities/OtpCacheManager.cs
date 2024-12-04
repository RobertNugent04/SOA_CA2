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
        private readonly ILogger<OtpCacheManager> _logger;

        public OtpCacheManager(ILogger<OtpCacheManager> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Stores an OTP for a user with an expiration time.
        /// </summary>
        public void StoreOtp(int userId, string otp, TimeSpan expiration)
        {
            try
            {
                _otpCache[userId] = (otp, DateTime.UtcNow.Add(expiration));
                _logger.LogInformation("Stored OTP for UserId {UserId}. Expires at {Expiration}", userId, DateTime.UtcNow.Add(expiration));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error storing OTP for UserId {UserId}", userId);
            }
        }

        /// <summary>
        /// Validates an OTP for a user by checking the stored value and expiration.
        /// </summary>
        public bool ValidateOtp(int userId, string otp)
        {
            try
            {
                if (_otpCache.TryGetValue(userId, out var storedOtp))
                {
                    if (storedOtp.Otp.Equals(otp, StringComparison.OrdinalIgnoreCase) && DateTime.UtcNow <= storedOtp.Expiration)
                    {
                        _logger.LogInformation("Successfully validated OTP for UserId {UserId}", userId);
                        return true;
                    }
                }

                _logger.LogWarning("Failed OTP validation for UserId {UserId}", userId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating OTP for UserId {UserId}", userId);
                return false;
            }
        }

        /// <summary>
        /// Invalidates an OTP by removing it from the cache.
        /// </summary>
        public void InvalidateOtp(int userId)
        {
            try
            {
                _otpCache.TryRemove(userId, out _);
                _logger.LogInformation("Invalidated OTP for UserId {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating OTP for UserId {UserId}", userId);
            }
        }

        /// <summary>
        /// Checks if a user has a valid OTP in the cache.
        /// </summary>
        public bool HasValidOtp(int userId)
        {
            try
            {
                if (_otpCache.TryGetValue(userId, out var storedOtp))
                {
                    bool isValid = DateTime.UtcNow <= storedOtp.Expiration;
                    _logger.LogInformation("Checked OTP for UserId {UserId}. IsValid: {IsValid}", userId, isValid);
                    return isValid;
                }

                _logger.LogWarning("No OTP found for UserId {UserId}", userId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking OTP for UserId {UserId}", userId);
                return false;
            }
        }
    }
}
