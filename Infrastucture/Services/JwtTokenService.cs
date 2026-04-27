using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CORE.Entities;
using Serilog;

namespace Infrastructure.Services;

/// <summary>
/// JWT Token Service - handles token generation, validation, and revocation
/// Generates both access tokens (short-lived, 15 min) and refresh tokens (long-lived, 7 days)
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generate both access and refresh tokens for a user
    /// </summary>
    Task<JwtTokens> GenerateTokensAsync(User user);

    /// <summary>
    /// Validate and get claims from a JWT token
    /// </summary>
    Task<ClaimsPrincipal?> ValidateTokenAsync(string token);

    /// <summary>
    /// Revoke a refresh token (add to blacklist)
    /// </summary>
    Task RevokeTokenAsync(string refreshToken);

    /// <summary>
    /// Check if a refresh token is revoked
    /// </summary>
    Task<bool> IsTokenRevokedAsync(string refreshToken);
}

/// <summary>
/// DTO for token pair
/// </summary>
public class JwtTokens
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public long ExpiresIn { get; set; }
}

/// <summary>
/// Implementation of JWT Token Service
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtTokenService> _logger;
    private static readonly HashSet<string> RevokedTokens = new(); // In-memory blacklist (replace with DB in production)
    private const int AccessTokenExpirationMinutes = 15;
    private const int RefreshTokenExpirationDays = 7;

    public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Generate JWT access token and refresh token
    /// </summary>
    public async Task<JwtTokens> GenerateTokensAsync(User user)
    {
        try
        {
            var jwtSecret = _configuration["Jwt:Secret"] 
                ?? throw new InvalidOperationException("JWT Secret not configured");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "E-Commerce-API";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "E-Commerce-App";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create claims for user
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.DisplayName),
                new(ClaimTypes.Role, user.Role.ToString()),
                new("IsEmailConfirmed", user.IsEmailConfirmed.ToString()),
            };

            // Generate Access Token (short-lived)
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes);
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = accessTokenExpiration,
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);

            // Generate Refresh Token (long-lived, random string)
            var refreshToken = GenerateRefreshToken();

            _logger.LogInformation(
                "✓ Generated tokens for user {UserId} ({Email}). Access token expires at {ExpiresAt}",
                user.Id,
                user.Email,
                accessTokenExpiration
            );

            return await Task.FromResult(new JwtTokens
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshToken,
                ExpiresIn = (long)(accessTokenExpiration - DateTime.UtcNow).TotalSeconds
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error generating tokens for user {UserId}", user.Id);
            throw;
        }
    }

    /// <summary>
    /// Validate JWT token and return claims
    /// </summary>
    public async Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
    {
        try
        {
            var jwtSecret = _configuration["Jwt:Secret"] 
                ?? throw new InvalidOperationException("JWT Secret not configured");
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false, // Set to true in production
                ValidateAudience = false, // Set to true in production
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return await Task.FromResult(principal);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ Token validation failed");
            return null;
        }
    }

    /// <summary>
    /// Revoke refresh token by adding to blacklist
    /// </summary>
    public async Task RevokeTokenAsync(string refreshToken)
    {
        RevokedTokens.Add(refreshToken);
        _logger.LogInformation("🔐 Refresh token revoked");
        await Task.CompletedTask;
    }

    /// <summary>
    /// Check if refresh token is in revoked list
    /// </summary>
    public async Task<bool> IsTokenRevokedAsync(string refreshToken)
    {
        return await Task.FromResult(RevokedTokens.Contains(refreshToken));
    }

    /// <summary>
    /// Generate random refresh token (256-bit)
    /// </summary>
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
