namespace CORE.Entities
{
    /// <summary>
    /// Product - represents a product available in the e-commerce store
    /// Contains product information, pricing, and availability
    /// </summary>
    public class Product
    {
        /// <summary>Unique identifier for the product</summary>
        public Guid Id { get; set; }

        /// <summary>Product name displayed in the store</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Detailed product description</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>Product price</summary>
        public decimal Price { get; set; } = decimal.Zero;

        /// <summary>Number of items available in stock</summary>
        public int Stock { get; set; } = 0;

        /// <summary>URL to product image</summary>
        public string? ImageUrl { get; set; }

        /// <summary>Product category (e.g., Electronics, Books, Clothing)</summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>Foreign key to Company</summary>
        public Guid CompanyId { get; set; }

        /// <summary>Navigation property - Company that sells this product</summary>
        public Company Company { get; set; } = null!;

        /// <summary>Timestamp when the product was created</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Navigation property - collection of cart items containing this product</summary>
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        /// <summary>Navigation property - collection of order items containing this product</summary>
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
