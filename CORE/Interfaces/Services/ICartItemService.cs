using CORE.Entities;
using CORE.Responses;

namespace CORE.Interfaces.Services
{
    /// <summary>
    /// Service interface for managing shopping cart items operations.
    /// Handles CRUD operations for cart items including validation and inventory checks.
    /// </summary>
    public interface ICartItemService
    {
        /// <summary>
        /// Get a specific cart item by its unique identifier.
        /// </summary>
        /// <param name="id">The cart item ID</param>
        /// <returns>Result containing the cart item or error message</returns>
        Task<Result<CartItem>> GetCartItemByIdAsync(Guid id);

        /// <summary>
        /// Get all cart items with pagination support.
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Result containing list of cart items or error message</returns>
        Task<Result<List<CartItem>>> GetAllCartItemsPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Get all items in a specific cart.
        /// </summary>
        /// <param name="id">The cart ID</param>
        /// <returns>Result containing list of cart items for the cart or error message</returns>
        Task<Result<List<CartItem>>> GetCartItemsByCartIdAsync(Guid id);

        /// <summary>
        /// Add a new item to the cart.
        /// </summary>
        /// <param name="cartItem">The cart item to add</param>
        /// <returns>Result containing the created cart item or error message</returns>
        Task<Result<CartItem>> AddCartItemAsync(CartItem cartItem);

        /// <summary>
        /// Update an existing cart item (e.g., change quantity).
        /// </summary>
        /// <param name="cartItem">The cart item with updated values</param>
        /// <returns>Result containing the updated cart item or error message</returns>
        Task<Result<CartItem>> UpdateCartItemAsync(CartItem cartItem);

        /// <summary>
        /// Delete a specific cart item.
        /// </summary>
        /// <param name="id">The cart item ID to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> DeleteCartItemAsync(Guid id);

        /// <summary>
        /// Clear all items from a specific cart.
        /// </summary>
        /// <param name="id">The cart ID</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> ClearCartItemsAsync(Guid id);

        /// <summary>
        /// Validate if a product can be added to cart based on inventory availability.
        /// </summary>
        /// <param name="product">The product to validate</param>
        /// <param name="quantity">The quantity to add</param>
        /// <returns>Result indicating if product can be added or error message</returns>
        Task<Result<bool>> CanAddToCartAsync(Product product, int quantity);

        /// <summary>
        /// Check if a cart item exists in the database.
        /// </summary>
        /// <param name="id">The cart item ID to check</param>
        /// <returns>Result indicating existence of cart item or error message</returns>
        Task<Result<bool>> CartItemExistsAsync(Guid id);
    }
}
