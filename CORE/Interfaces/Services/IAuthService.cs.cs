
using CORE.Responses;

namespace CORE.Interfaces.Services
{
    /// <summary>
    /// Service interface for authentication and authorization operations.
    /// Handles user registration, login, token management, and account verification.
    /// </summary>
    public interface IAuthService
    {
        // ========== AUTHENTICATION OPERATIONS ==========

        /// <summary>
        /// Authenticate user with email and password.
        /// Returns JWT token on successful authentication.
        /// </summary>
        /// <param name="email">The user email</param>
        /// <param name="password">The user password</param>
        /// <returns>Result containing JWT token or error message</returns>
        Task<Result<string>> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Register a new user account.
        /// Creates user and returns authentication token.
        /// </summary>
        /// <param name="email">The user email</param>
        /// <param name="password">The user password</param>
        /// <param name="displayName">The user display name</param>
        /// <returns>Result containing JWT token and user info or error message</returns>
        Task<Result<string>> RegisterAsync(string email, string password, string displayName);

        /// <summary>
        /// Refresh expired JWT token using refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token</param>
        /// <returns>Result containing new JWT token or error message</returns>
        Task<Result<string>> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Logout user (invalidate tokens).
        /// </summary>
        /// <param name="userId">The user ID to logout</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> LogoutAsync(Guid userId);

        // ========== USER VERIFICATION ==========

        /// <summary>
        /// Verify if user email exists in system.
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns>Result indicating if email exists or error message</returns>
        Task<Result<bool>> UserExistsAsync(string email);

        /// <summary>
        /// Verify if user email is confirmed.
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>Result indicating if email is confirmed or error message</returns>
        Task<Result<bool>> IsEmailConfirmedAsync(Guid userId);

        /// <summary>
        /// Check if user account is active.
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>Result indicating if user is active or error message</returns>
        Task<Result<bool>> IsUserActiveAsync(Guid userId);

        // ========== PASSWORD OPERATIONS ==========

        /// <summary>
        /// Change user password (requires current password verification).
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="currentPassword">The current password</param>
        /// <param name="newPassword">The new password</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);

        /// <summary>
        /// Reset user password (forgot password flow).
        /// Generates reset token.
        /// </summary>
        /// <param name="email">The user email</param>
        /// <returns>Result containing reset token or error message</returns>
        Task<Result<string>> GeneratePasswordResetTokenAsync(string email);

        /// <summary>
        /// Confirm password reset with token.
        /// </summary>
        /// <param name="email">The user email</param>
        /// <param name="resetToken">The reset token</param>
        /// <param name="newPassword">The new password</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> ResetPasswordAsync(string email, string resetToken, string newPassword);

        // ========== EMAIL VERIFICATION ==========

        /// <summary>
        /// Generate email confirmation token.
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>Result containing confirmation token or error message</returns>
        Task<Result<string>> GenerateEmailConfirmationTokenAsync(Guid userId);

        /// <summary>
        /// Confirm user email with verification token.
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="confirmationToken">The confirmation token</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> ConfirmEmailAsync(Guid userId, string confirmationToken);
    }
}
