using CORE.Entities;
using CORE.Responses;

namespace CORE.Interfaces.Services
{
    /// <summary>
    /// Service interface for managing products.
    /// Handles product CRUD operations, inventory management, and product discovery.
    /// </summary>
    public interface IProductService
    {
        // ========== READ OPERATIONS ==========

        /// <summary>
        /// Get product by its ID.
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>Result containing the product or error message</returns>
        Task<Result<Product>> GetProductByIdAsync(Guid id);

        /// <summary>
        /// Get all products with pagination.
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Result containing paginated products or error message</returns>
        Task<Result<List<Product>>> GetAllProductsPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Search products by name.
        /// </summary>
        /// <param name="name">Product name or partial name</param>
        /// <returns>Result containing matching products or error message</returns>
        Task<Result<List<Product>>> SearchProductByNameAsync(string name);

        /// <summary>
        /// Get total count of products.
        /// </summary>
        /// <returns>Result containing product count or error message</returns>
        Task<Result<int>> GetProductCountAsync();

        // ========== WRITE OPERATIONS ==========

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>Result containing the created product or error message</returns>
        Task<Result<Product>> CreateProductAsync(Product product);

        /// <summary>
        /// Update an existing product.
        /// </summary>
        /// <param name="product">The product with updated values</param>
        /// <returns>Result containing the updated product or error message</returns>
        Task<Result<Product>> UpdateProductAsync(Product product);

        /// <summary>
        /// Delete a product by ID.
        /// </summary>
        /// <param name="id">The product ID to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> DeleteProductAsync(Guid id);

        // ========== INVENTORY OPERATIONS ==========

        /// <summary>
        /// Update product stock quantity.
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="quantity">The new stock quantity</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> UpdateProductStockAsync(Guid productId, int quantity);

        /// <summary>
        /// Decrease product stock (after purchase).
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="quantity">The quantity to decrease</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> DecreaseProductStockAsync(Guid productId, int quantity);

        /// <summary>
        /// Get current stock quantity for product.
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <returns>Result containing stock quantity or error message</returns>
        Task<Result<int>> GetProductStockAsync(Guid productId);

        // ========== VALIDATION OPERATIONS ==========

        /// <summary>
        /// Check if product exists.
        /// </summary>
        /// <param name="id">The product ID to check</param>
        /// <returns>Result indicating existence or error message</returns>
        Task<Result<bool>> ProductExistsAsync(Guid id);

        /// <summary>
        /// Check if product has sufficient stock.
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="quantity">The required quantity</param>
        /// <returns>Result indicating if stock is available or error message</returns>
        Task<Result<bool>> HasSufficientStockAsync(Guid productId, int quantity);

        /// <summary>
        /// Check if product is available for sale.
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <returns>Result indicating if product is available or error message</returns>
        Task<Result<bool>> IsProductAvailableAsync(Guid productId);
    }
}
