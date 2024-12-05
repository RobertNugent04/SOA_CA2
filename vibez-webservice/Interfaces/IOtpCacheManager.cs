namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for managing OTP storage and validation.
    /// </summary>
    public interface IOtpCacheManager
    {
        /// <summary>
        /// Stores an OTP for a user.
        /// </summary>
        void StoreOtp(int userId, string otp, TimeSpan expiration);

        /// <summary>
        /// Validates an OTP for a user.
        /// </summary>
        bool ValidateOtp(int userId, string otp);

        /// <summary>
        /// Invalidates an OTP for a user.
        /// </summary>
        void InvalidateOtp(int userId);

        /// <summary>
        /// Checks if a user has a valid OTP.
        /// </summary>
        bool HasValidOtp(int userId);
    }
}
