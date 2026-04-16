using CORE.Entities;
using CORE.Responses;

namespace CORE.Interfaces.Services
{
    /// <summary>
    /// Service interface for managing shopping carts.
    /// Handles cart operations including item management and calculations.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Get cart by its ID.
        /// </summary>
        /// <param name="id">The cart ID</param>
        /// <returns>Result containing the cart with items or error message</returns>
        Task<Result<Cart>> GetCartByIdAsync(Guid id);

        /// <summary>
        /// Get cart for a specific user.
        /// Each user typically has one active cart.
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>Result containing user's cart or error message</returns>
        Task<Result<Cart>> GetCartByUserIdAsync(Guid userId);

        /// <summary>
        /// Get all carts with pagination.
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Result containing list of carts or error message</returns>
        Task<Result<List<Cart>>> GetAllCartsPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Get total price of all items in cart.
        /// </summary>
        /// <param name="id">The cart ID</param>
        /// <returns>Result containing total price or error message</returns>
        Task<Result<decimal>> GetCartTotalAsync(Guid id);

        /// <summary>
        /// Get total item count in cart.
        /// </summary>
        /// <param name="id">The cart ID</param>
        /// <returns>Result containing item count or error message</returns>
        Task<Result<int>> GetCartItemCountAsync(Guid id);

        /// <summary>
        /// Create a new cart for a user.
        /// </summary>
        /// <param name="cart">The cart to create</param>
        /// <returns>Result containing the created cart or error message</returns>
        Task<Result<Cart>> CreateCartAsync(Cart cart);

        /// <summary>
        /// Add item to cart.
        /// </summary>
        /// <param name="cartId">The cart ID</param>
        /// <param name="cartItem">The cart item to add</param>
        /// <returns>Result containing updated cart or error message</returns>
        Task<Result<Cart>> AddItemToCartAsync(Guid cartId, CartItem cartItem);

        /// <summary>
        /// Remove item from cart.
        /// </summary>
        /// <param name="cartId">The cart ID</param>
        /// <param name="cartItemId">The cart item ID to remove</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> RemoveItemFromCartAsync(Guid cartId, Guid cartItemId);

        /// <summary>
        /// Update cart metadata.
        /// </summary>
        /// <param name="cart">The cart with updated values</param>
        /// <returns>Result containing updated cart or error message</returns>
        Task<Result<Cart>> UpdateCartAsync(Cart cart);

        /// <summary>
        /// Clear all items from cart without deleting it.
        /// </summary>
        /// <param name="id">The cart ID</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> ClearCartAsync(Guid id);

        /// <summary>
        /// Delete entire cart.
        /// </summary>
        /// <param name="id">The cart ID to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> DeleteCartByIdAsync(Guid id);
    }
}
