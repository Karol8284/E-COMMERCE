namespace CORE.Entities
{
    /// <summary>
    /// CartItem - represents a single product in a user's shopping cart
    /// </summary>
    public class CartItem
    {
        /// <summary>Unique identifier for the cart item</summary>
        public Guid Id { get; set; }

        /// <summary>Foreign key to Cart</summary>
        public Guid CartId { get; set; }

        /// <summary>Foreign key to Product</summary>
        public Guid ProductId { get; set; }

        /// <summary>Quantity of the product in the cart</summary>
        public int Quantity { get; set; }

        /// <summary>Timestamp when the item was added to the cart</summary>
        public DateTime AddedAt { get; set; }

        /// <summary>Navigation property - Cart containing this item</summary>
        public Cart Cart { get; set; } = null!;

        /// <summary>Navigation property - Product in this cart item</summary>
        public Product Product { get; set; } = null!;
    }
}
