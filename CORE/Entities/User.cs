using CORE.Enums;

namespace CORE.Entities
{
    /// <summary>
    /// User - represents an application user with authentication and profile information
    /// Aggregate root for user account management and order/cart tracking
    /// </summary>
    public class User
    {
        /// <summary>Unique identifier for the user (primary key)</summary>
        public Guid Id { get; set; }

        /// <summary>User's email address; used for login and communication (must be unique)</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>BCrypt hashed password for secure authentication</summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>User's display name shown in the application (e.g., "John Doe")</summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>Optional URL to user's profile avatar/profile picture</summary>
        public string? AvatarUrl { get; set; }

        /// <summary>User's role determining permissions (Admin, User)</summary>
        public Role Role { get; set; }

        /// <summary>Indicates whether the user account is active or deactivated</summary>
        public bool IsActive { get; set; }

        /// <summary>Indicates whether the user's email address has been verified</summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>Timestamp when the user account was created in UTC</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Navigation property - collection of carts for this user</summary>
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

        /// <summary>Navigation property - collection of orders for this user</summary>
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}