namespace CORE.Entities
{
    /// <summary>
    /// Cart - represents a user's shopping cart containing items before checkout
    /// </summary>
    public class Cart
    {
        /// <summary>Unique identifier for the cart</summary>
        public Guid Id { get; set; }

        /// <summary>Foreign key to User - owner of the cart</summary>
        public Guid UserId { get; set; }

        /// <summary>Navigation property - User who owns this cart</summary>
        public User User { get; set; } = null!;

        /// <summary>Timestamp when the cart was created</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Timestamp when the cart was last updated</summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>Navigation property - collection of items in the cart</summary>
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
