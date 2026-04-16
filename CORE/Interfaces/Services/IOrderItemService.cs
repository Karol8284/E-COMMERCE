
using CORE.Entities;
using CORE.Responses;

namespace CORE.Interfaces.Services
{
    /// <summary>
    /// Service interface for managing order items.
    /// Handles CRUD operations for individual items within orders.
    /// </summary>
    public interface IOrderItemService
    {
        // ========== READ OPERATIONS ==========

        /// <summary>
        /// Get a specific order item by its ID.
        /// </summary>
        /// <param name="id">The order item ID</param>
        /// <returns>Result containing the order item or error message</returns>
        Task<Result<OrderItem>> GetOrderItemByIdAsync(Guid id);

        /// <summary>
        /// Get all items in a specific order.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Result containing list of order items or error message</returns>
        Task<Result<List<OrderItem>>> GetOrderItemsByOrderIdAsync(Guid orderId);

        /// <summary>
        /// Get all order items with pagination.
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Result containing paginated order items or error message</returns>
        Task<Result<List<OrderItem>>> GetAllOrderItemsPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Get total count of order items.
        /// </summary>
        /// <returns>Result containing total count or error message</returns>
        Task<Result<int>> GetOrderItemCountAsync();

        // ========== WRITE OPERATIONS ==========

        /// <summary>
        /// Create a new order item.
        /// </summary>
        /// <param name="orderItem">The order item to create</param>
        /// <returns>Result containing the created order item or error message</returns>
        Task<Result<OrderItem>> CreateOrderItemAsync(OrderItem orderItem);

        /// <summary>
        /// Update an existing order item (e.g., quantity or price adjustments).
        /// </summary>
        /// <param name="orderItem">The order item with updated values</param>
        /// <returns>Result containing the updated order item or error message</returns>
        Task<Result<OrderItem>> UpdateOrderItemAsync(OrderItem orderItem);

        /// <summary>
        /// Delete an order item by ID.
        /// </summary>
        /// <param name="id">The order item ID to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> DeleteOrderItemAsync(Guid id);

        /// <summary>
        /// Delete all items from a specific order.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> DeleteOrderItemsByOrderIdAsync(Guid orderId);

        // ========== VALIDATION OPERATIONS ==========

        /// <summary>
        /// Check if an order item exists.
        /// </summary>
        /// <param name="id">The order item ID to check</param>
        /// <returns>Result indicating existence or error message</returns>
        Task<Result<bool>> OrderItemExistsAsync(Guid id);

        /// <summary>
        /// Check if order has any items.
        /// </summary>
        /// <param name="orderId">The order ID to check</param>
        /// <returns>Result indicating if order has items or error message</returns>
        Task<Result<bool>> OrderHasItemsAsync(Guid orderId);

        // ========== CALCULATION OPERATIONS ==========

        /// <summary>
        /// Calculate total price for all items in an order.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Result containing total order amount or error message</returns>
        Task<Result<decimal>> GetOrderTotalAsync(Guid orderId);

        /// <summary>
        /// Get total quantity of items in an order.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Result containing total item quantity or error message</returns>
        Task<Result<int>> GetOrderItemQuantityAsync(Guid orderId);
    }
}
