using CORE.Entities;
using CORE.Responses;

namespace CORE.Interfaces.Services
{
    /// <summary>
    /// Service interface for managing companies.
    /// Handles company CRUD operations, validations, and business logic.
    /// </summary>
    public interface ICompanyService
    {
        // ========== READ OPERATIONS ==========

        /// <summary>
        /// Get company by its unique identifier.
        /// </summary>
        /// <param name="id">The company ID</param>
        /// <returns>Result containing the company or error message</returns>
        Task<Result<Company>> GetCompanyByIdAsync(Guid id);

        /// <summary>
        /// Get all companies with pagination support.
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Result containing list of companies or error message</returns>
        Task<Result<List<Company>>> GetAllCompaniesPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Get company by its name.
        /// </summary>
        /// <param name="name">The company name</param>
        /// <returns>Result containing the company or error message</returns>
        Task<Result<Company>> GetCompanyByNameAsync(string name);

        /// <summary>
        /// Get total count of all companies.
        /// </summary>
        /// <returns>Result containing total company count or error message</returns>
        Task<Result<int>> GetCompanyCountAsync();

        // ========== WRITE OPERATIONS ==========

        /// <summary>
        /// Create a new company.
        /// </summary>
        /// <param name="company">The company entity to create</param>
        /// <returns>Result containing the created company or error message</returns>
        Task<Result<Company>> CreateCompanyAsync(Company company);

        /// <summary>
        /// Update an existing company.
        /// </summary>
        /// <param name="company">The company with updated values</param>
        /// <returns>Result containing the updated company or error message</returns>
        Task<Result<Company>> UpdateCompanyAsync(Company company);

        /// <summary>
        /// Delete a company by ID.
        /// </summary>
        /// <param name="id">The company ID to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> DeleteCompanyAsync(Guid id);

        // ========== VALIDATION OPERATIONS ==========

        /// <summary>
        /// Check if a company exists by ID.
        /// </summary>
        /// <param name="id">The company ID to check</param>
        /// <returns>Result indicating existence or error message</returns>
        Task<Result<bool>> CompanyExistsAsync(Guid id);

        /// <summary>
        /// Validate if a company name is unique in the system.
        /// Used to prevent duplicate company names.
        /// </summary>
        /// <param name="name">The company name to validate</param>
        /// <param name="excludeId">Optional: Exclude specific company ID (useful for update operations)</param>
        /// <returns>Result indicating if name is unique or error message</returns>
        Task<Result<bool>> IsCompanyNameUniqueAsync(string name, Guid? excludeId = null);
    }
}
