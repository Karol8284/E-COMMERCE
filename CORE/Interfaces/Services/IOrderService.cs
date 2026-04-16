using CORE.Entities;
using CORE.Responses;

namespace CORE.Interfaces.Services
{
    /// <summary>
    /// Service interface for managing orders.
    /// Handles order creation, status management, and order lifecycle operations.
    /// </summary>
    public interface IOrderService
    {
        // ========== READ OPERATIONS ==========

        /// <summary>
        /// Get order by its ID.
        /// </summary>
        /// <param name="id">The order ID</param>
        /// <returns>Result containing the order with items or error message</returns>
        Task<Result<Order>> GetOrderByIdAsync(Guid id);

        /// <summary>
        /// Get all orders for a specific user.
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>Result containing user's orders or error message</returns>
        Task<Result<List<Order>>> GetUserOrdersAsync(Guid userId);

        /// <summary>
        /// Get all orders with pagination.
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Result containing paginated orders or error message</returns>
        Task<Result<List<Order>>> GetAllOrdersPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Get orders by status.
        /// </summary>
        /// <param name="status">The order status</param>
        /// <returns>Result containing orders with specified status or error message</returns>
        Task<Result<List<Order>>> GetOrdersByStatusAsync(string status);

        /// <summary>
        /// Get total count of orders.
        /// </summary>
        /// <returns>Result containing order count or error message</returns>
        Task<Result<int>> GetOrderCountAsync();

        // ========== WRITE OPERATIONS ==========

        /// <summary>
        /// Create new order from user's cart.
        /// </summary>
        /// <param name="order">The order to create</param>
        /// <returns>Result containing the created order or error message</returns>
        Task<Result<Order>> CreateOrderAsync(Order order);

        /// <summary>
        /// Update an existing order.
        /// </summary>
        /// <param name="order">The order with updated values</param>
        /// <returns>Result containing the updated order or error message</returns>
        Task<Result<Order>> UpdateOrderAsync(Order order);

        /// <summary>
        /// Cancel an order.
        /// </summary>
        /// <param name="orderId">The order ID to cancel</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> CancelOrderAsync(Guid orderId);

        /// <summary>
        /// Delete order (admin operation).
        /// </summary>
        /// <param name="id">The order ID to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result<bool>> DeleteOrderAsync(Guid id);

        // ========== STATUS OPERATIONS ==========

        /// <summary>
        /// Update order status.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="status">The new status</param>
        /// <returns>Result containing updated order or error message</returns>
        Task<Result<Order>> UpdateOrderStatusAsync(Guid orderId, string status);

        /// <summary>
        /// Get current order status.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Result containing order status or error message</returns>
        Task<Result<string>> GetOrderStatusAsync(Guid orderId);

        // ========== CALCULATIONS ==========

        /// <summary>
        /// Get order total amount.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Result containing total amount or error message</returns>
        Task<Result<decimal>> GetOrderTotalAsync(Guid orderId);

        // ========== VALIDATION OPERATIONS ==========

        /// <summary>
        /// Check if order exists.
        /// </summary>
        /// <param name="id">The order ID to check</param>
        /// <returns>Result indicating existence or error message</returns>
        Task<Result<bool>> OrderExistsAsync(Guid id);

        /// <summary>
        /// Check if order can be cancelled.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Result indicating if cancellation is allowed or error message</returns>
        Task<Result<bool>> CanCancelOrderAsync(Guid orderId);
    }
}
