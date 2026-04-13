namespace CORE.Entities
{
    /// <summary>
    /// OrderItem - represents a single product line item within an order
    /// </summary>
    public class OrderItem
    {
        /// <summary>Unique identifier for the order item</summary>
        public Guid Id { get; set; }

        /// <summary>Foreign key to Order</summary>
        public Guid OrderId { get; set; }

        /// <summary>Foreign key to Product</summary>
        public Guid ProductId { get; set; }

        /// <summary>Quantity of the product in this order item</summary>
        public int Quantity { get; set; }

        /// <summary>Unit price at the time of purchase (may differ from current product price)</summary>
        public decimal Price { get; set; }

        /// <summary>Navigation property - Order containing this item</summary>
        public Order Order { get; set; } = null!;

        /// <summary>Navigation property - Product in this order item</summary>
        public Product Product { get; set; } = null!;
    }
}
