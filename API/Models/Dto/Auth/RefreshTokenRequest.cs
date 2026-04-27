namespace API.Models.Dto.Auth;

/// <summary>
/// Request to refresh an access token using a valid refresh token
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// The refresh token from previous login/refresh
    /// </summary>
    public required string RefreshToken { get; set; }
}
