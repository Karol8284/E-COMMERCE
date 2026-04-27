using API.Filters;
using API.Models.Dto.Auth;
using API.Models.Dto.User;
using CORE.Entities;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ValidationFilter]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, IJwtTokenService jwtTokenService, ILogger<AuthController> logger)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == registerDto.Email.ToLower());

            if (existingUser != null)
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "User with this email already exists"
                });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = CORE.Enums.Role.User,
                IsActive = true,
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Create a cart for the new user
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                IsEmailConfirmed = user.IsEmailConfirmed,
                CreatedAt = user.CreatedAt
            };

            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful",
                User = new AuthUserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Role = user.Role.ToString()
                }
            });
        }

        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower());

            if (user == null)
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                });

            if (!user.IsActive)
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "User account is inactive"
                });

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                });

            // Generate JWT tokens
            try
            {
                var tokens = await _jwtTokenService.GenerateTokensAsync(user);
                _logger.LogInformation("JWT tokens generated successfully for user {UserId}", user.Id);

                return Ok(new
                {
                    success = true,
                    message = "Login successful",
                    accessToken = tokens.AccessToken,
                    refreshToken = tokens.RefreshToken,
                    expiresIn = 900, // 15 minutes in seconds
                    user = new AuthUserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Role = user.Role.ToString()
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT tokens for user {UserId}", user.Id);
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "An error occurred during token generation"
                });
            }
        }

        /// <summary>
        /// Get current user profile
        /// </summary>
        [HttpGet("profile/{userId}")]
        public async Task<ActionResult<UserDto>> GetProfile(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return NotFound("User not found");

            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                IsEmailConfirmed = user.IsEmailConfirmed,
                CreatedAt = user.CreatedAt
            });
        }

        /// <summary>
        /// Refresh JWT access token using a valid refresh token
        /// </summary>
        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { success = false, message = "Refresh token is required" });

            try
            {
                // Validate refresh token - check if revoked
                var isRevoked = await _jwtTokenService.IsTokenRevokedAsync(request.RefreshToken);
                if (isRevoked)
                {
                    _logger.LogWarning("Attempted to refresh with revoked token");
                    return Unauthorized(new { success = false, message = "Refresh token is revoked" });
                }

                // Validate and extract claims from refresh token
                var principal = await _jwtTokenService.ValidateTokenAsync(request.RefreshToken);
                if (principal == null)
                {
                    _logger.LogWarning("Invalid or expired refresh token");
                    return Unauthorized(new { success = false, message = "Invalid or expired refresh token" });
                }

                // Extract user ID from claims
                var userIdClaim = principal.FindFirst("sub")?.Value ?? principal.FindFirst("nameid")?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    _logger.LogWarning("Could not extract user ID from token claims");
                    return Unauthorized(new { success = false, message = "Invalid token claims" });
                }

                // Get user from database
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found for token refresh", userId);
                    return Unauthorized(new { success = false, message = "User not found" });
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning("Inactive user {UserId} attempted token refresh", userId);
                    return Unauthorized(new { success = false, message = "User account is inactive" });
                }

                // Generate new token pair
                var newTokens = await _jwtTokenService.GenerateTokensAsync(user);
                
                // Revoke old refresh token
                await _jwtTokenService.RevokeTokenAsync(request.RefreshToken);
                _logger.LogInformation("Tokens refreshed successfully for user {UserId}", userId);

                return Ok(new
                {
                    success = true,
                    message = "Token refreshed successfully",
                    accessToken = newTokens.AccessToken,
                    refreshToken = newTokens.RefreshToken,
                    expiresIn = 900, // 15 minutes in seconds
                    user = new AuthUserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Role = user.Role.ToString()
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(500, new { success = false, message = "An error occurred during token refresh" });
            }
        }
    }
}
