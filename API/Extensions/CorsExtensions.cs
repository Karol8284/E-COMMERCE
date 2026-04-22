using API.Configuration;

namespace API.Extensions;

/// <summary>
/// Extension methods for CORS configuration
/// Implements Clean Code pattern - moves configuration logic out of Program.cs
/// 
/// Usage: builder.Services.AddCustomCors(builder.Configuration);
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Adds CORS services with configuration from appsettings.json
    /// 
    /// Best practices implemented:
    /// 1. Configuration-driven (supports multiple environments)
    /// 2. Validates CORS settings at startup
    /// 3. Uses builder pattern for flexibility
    /// 4. Separates concerns from Program.cs
    /// </summary>
    public static IServiceCollection AddCustomCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Read CORS settings from configuration
        var corsSettings = new CorsSettings();
        configuration.GetSection("CorsSettings").Bind(corsSettings);

        // Validate settings at startup (fail fast)
        corsSettings.Validate();

        // Register settings as singleton for potential logging/monitoring
        services.AddSingleton(corsSettings);

        // Add CORS policy
        services.AddCors(options =>
        {
            options.AddPolicy(corsSettings.PolicyName, policy =>
            {
                // Configure allowed origins
                policy.WithOrigins(corsSettings.AllowedOrigins.ToArray());

                // Configure allowed methods
                policy.WithMethods(corsSettings.AllowedMethods.ToArray());

                // Configure allowed headers
                if (corsSettings.AllowedHeaders.Contains("*"))
                    policy.AllowAnyHeader();
                else
                    policy.WithHeaders(corsSettings.AllowedHeaders.ToArray());

                // Configure credentials
                if (corsSettings.AllowCredentials)
                    policy.AllowCredentials();
                else
                    policy.DisallowCredentials();

                // Configure cache time for preflight requests
                policy.SetPreflightMaxAge(TimeSpan.FromSeconds(corsSettings.MaxAgeSeconds));

                // Configure exposed headers (if any)
                if (corsSettings.ExposedHeaders.Any())
                    policy.WithExposedHeaders(corsSettings.ExposedHeaders.ToArray());
            });
        });

        return services;
    }
}
