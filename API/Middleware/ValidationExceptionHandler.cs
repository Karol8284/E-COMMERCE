using FluentValidation;
using API.Models;

namespace API.Middleware;

/// <summary>
/// Extension to handle FluentValidation errors
/// </summary>
public static class FluentValidationExtensions
{
    /// <summary>
    /// Convert FluentValidation failures to ApiResponse error details
    /// </summary>
    public static List<ErrorDetail> ToErrorDetails(this FluentValidation.Results.ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .Select(g => new ErrorDetail(
                string.Join("; ", g.Select(e => e.ErrorMessage)),
                g.Key,
                "VALIDATION_ERROR"
            ))
            .ToList();
    }
}
