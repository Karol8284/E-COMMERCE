namespace API.Configuration;

/// <summary>
/// CORS configuration settings from appsettings.json
/// Supports different CORS policies per environment (Dev, Staging, Production)
/// 
/// Usage in appsettings.Development.json:
/// {
///   "CorsSettings": {
///     "PolicyName": "AllowFrontend",
///     "AllowedOrigins": ["http://localhost:5173", "http://localhost:3000"],
///     "AllowCredentials": true,
///     "AllowedMethods": ["GET", "POST", "PUT", "DELETE", "OPTIONS"],
///     "AllowedHeaders": ["*"],
///     "MaxAgeSeconds": 3600,
///     "ExposedHeaders": ["X-Total-Count", "X-Page-Number"]
///   }
/// }
/// </summary>
public class CorsSettings
{
    /// <summary>
    /// Name of the CORS policy (e.g., "AllowFrontend")
    /// </summary>
    public string PolicyName { get; set; } = "AllowFrontend";

    /// <summary>
    /// List of allowed origins (e.g., http://localhost:5173)
    /// Use specific URLs for production, never use "*" with credentials
    /// </summary>
    public List<string> AllowedOrigins { get; set; } = new();

    /// <summary>
    /// Allow credentials (cookies, auth headers)
    /// WARNING: Cannot be used with AllowAnyOrigin()
    /// </summary>
    public bool AllowCredentials { get; set; } = false;

    /// <summary>
    /// HTTP methods allowed in CORS requests
    /// </summary>
    public List<string> AllowedMethods { get; set; } = new() { "GET", "POST", "PUT", "DELETE", "OPTIONS" };

    /// <summary>
    /// Headers allowed in requests (use ["*"] only in development)
    /// </summary>
    public List<string> AllowedHeaders { get; set; } = new() { "*" };

    /// <summary>
    /// Maximum time (in seconds) browser can cache preflight requests
    /// Reduces preflight overhead
    /// </summary>
    public int MaxAgeSeconds { get; set; } = 3600;

    /// <summary>
    /// Headers exposed to the browser (for custom response headers)
    /// Example: Pagination headers, custom error headers
    /// </summary>
    public List<string> ExposedHeaders { get; set; } = new();

    /// <summary>
    /// Validates CORS settings for security
    /// </summary>
    public void Validate()
    {
        if (!AllowedOrigins.Any())
            throw new InvalidOperationException("CORS: AllowedOrigins cannot be empty");

        if (AllowCredentials && AllowedOrigins.Contains("*"))
            throw new InvalidOperationException("CORS: Cannot use AllowCredentials=true with AllowAnyOrigin (*)");

        if (!AllowedMethods.Any())
            throw new InvalidOperationException("CORS: AllowedMethods cannot be empty");
    }
}
