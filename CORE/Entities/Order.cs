using CORE.Enums;

namespace CORE.Entities
{
    /// <summary>
    /// Order - represents a completed purchase order with order items and delivery information
    /// </summary>
    public class Order
    {
        /// <summary>Unique identifier for the order</summary>
        public Guid Id { get; set; }

        /// <summary>Foreign key to User - customer who placed the order</summary>
        public Guid UserId { get; set; }

        /// <summary>Navigation property - User who placed this order</summary>
        public User User { get; set; } = null!;

        /// <summary>Current status of the order (Pending, Shipped, Delivered, Cancelled)</summary>
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        /// <summary>Total price of the order</summary>
        public decimal TotalPrice { get; set; }

        /// <summary>Shipping address for the order</summary>
        public string ShippingAddress { get; set; } = string.Empty;

        /// <summary>Timestamp when the order was created</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Expected or actual delivery date</summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>Timestamp when the order was cancelled (if applicable)</summary>
        public DateTime? CancelledAt { get; set; }

        /// <summary>Navigation property - collection of items in this order</summary>
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
