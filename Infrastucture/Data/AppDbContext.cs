using CORE.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    /// <summary>
    /// AppDbContext - main database context for the e-commerce application
    /// Configures all entities, relationships, and database mappings
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Company> Companies { get; set; }

        /// <summary>
        /// Configures entity relationships and constraints using Fluent API
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.DisplayName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Role).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
                
                // Relationships
                entity.HasMany(u => u.Carts)
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Orders)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Company Configuration
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(255);

                // Relationships
                entity.HasMany<Product>()
                    .WithOne(p => p.Company)
                    .HasForeignKey(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Description).IsRequired().HasMaxLength(2000);
                entity.Property(p => p.Price).HasPrecision(18, 2);
                entity.Property(p => p.Stock).IsRequired();
                entity.Property(p => p.Category).IsRequired().HasMaxLength(100);
                entity.Property(p => p.CompanyId).IsRequired();

                // Relationships
                entity.HasMany(p => p.CartItems)
                    .WithOne(ci => ci.Product)
                    .HasForeignKey(ci => ci.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.OrderItems)
                    .WithOne(oi => oi.Product)
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Cart Configuration
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.UserId).IsRequired();
                entity.Property(c => c.CreatedAt).IsRequired();

                // Relationships
                entity.HasMany(c => c.Items)
                    .WithOne(ci => ci.Cart)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CartItem Configuration
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);
                entity.Property(ci => ci.CartId).IsRequired();
                entity.Property(ci => ci.ProductId).IsRequired();
                entity.Property(ci => ci.Quantity).IsRequired();
                entity.Property(ci => ci.AddedAt).IsRequired();

                // Composite index for unique cart items
                entity.HasIndex(ci => new { ci.CartId, ci.ProductId }).IsUnique();
            });

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.UserId).IsRequired();
                entity.Property(o => o.Status).IsRequired();
                entity.Property(o => o.TotalPrice).HasPrecision(18, 2).IsRequired();
                entity.Property(o => o.ShippingAddress).IsRequired().HasMaxLength(500);
                entity.Property(o => o.CreatedAt).IsRequired();

                // Relationships
                entity.HasMany(o => o.Items)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem Configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.OrderId).IsRequired();
                entity.Property(oi => oi.ProductId).IsRequired();
                entity.Property(oi => oi.Quantity).IsRequired();
                entity.Property(oi => oi.Price).HasPrecision(18, 2).IsRequired();
            });
        }
    }

    /// <summary>
    /// Extension methods for AppDbContext
    /// </summary>
    public static class AppDbContextExtensions
    {
        /// <summary>
        /// Seeds initial data for testing and demonstration purposes
        /// </summary>
        public static void SeedData(this AppDbContext context)
        {
            // Check if data already exists
            if (context.Companies.Any() || context.Users.Any() || context.Products.Any())
                return;

            // Create Companies
            var techStoreId = Guid.NewGuid();
            var bookStoreId = Guid.NewGuid();

            var companies = new List<Company>
            {
                new() { Id = techStoreId, Name = "TechHub Store", Email = "contact@techhub.com" },
                new() { Id = bookStoreId, Name = "BookWorld", Email = "support@bookworld.com" }
            };

            context.Companies.AddRange(companies);

            // Create Products
            var products = new List<Product>
            {
                new() 
                { 
                    Id = Guid.NewGuid(),
                    Name = "Laptop Pro 15", 
                    Description = "High-performance laptop with 16GB RAM and 512GB SSD. Perfect for professionals and developers.", 
                    Price = 1299.99m, 
                    Stock = 50,
                    ImageUrl = "https://via.placeholder.com/300?text=Laptop",
                    Category = "Electronics", 
                    CompanyId = techStoreId, 
                    CreatedAt = DateTime.UtcNow 
                },
                new() 
                { 
                    Id = Guid.NewGuid(),
                    Name = "Wireless Mouse", 
                    Description = "Ergonomic wireless mouse with 2.4GHz receiver and 18-month battery life.", 
                    Price = 29.99m, 
                    Stock = 200,
                    ImageUrl = "https://via.placeholder.com/300?text=Mouse",
                    Category = "Electronics", 
                    CompanyId = techStoreId, 
                    CreatedAt = DateTime.UtcNow 
                },
                new() 
                { 
                    Id = Guid.NewGuid(),
                    Name = "USB-C Cable 2m", 
                    Description = "Durable USB-C to USB-C cable, 100W power delivery, ideal for charging and data transfer.", 
                    Price = 14.99m, 
                    Stock = 500,
                    ImageUrl = "https://via.placeholder.com/300?text=Cable",
                    Category = "Electronics", 
                    CompanyId = techStoreId, 
                    CreatedAt = DateTime.UtcNow 
                },
                new() 
                { 
                    Id = Guid.NewGuid(),
                    Name = "Clean Code", 
                    Description = "A Handbook of Agile Software Craftsmanship by Robert C. Martin. Essential reading for developers.", 
                    Price = 32.99m, 
                    Stock = 120,
                    ImageUrl = "https://via.placeholder.com/300?text=CleanCode",
                    Category = "Books", 
                    CompanyId = bookStoreId, 
                    CreatedAt = DateTime.UtcNow 
                },
                new() 
                { 
                    Id = Guid.NewGuid(),
                    Name = "The Pragmatic Programmer", 
                    Description = "Your Journey to Mastery in Software Development. Updated for modern programming practices.", 
                    Price = 39.99m, 
                    Stock = 85,
                    ImageUrl = "https://via.placeholder.com/300?text=Pragmatic",
                    Category = "Books", 
                    CompanyId = bookStoreId, 
                    CreatedAt = DateTime.UtcNow 
                },
                new() 
                { 
                    Id = Guid.NewGuid(),
                    Name = "Design Patterns", 
                    Description = "Elements of Reusable Object-Oriented Software. The classic reference book for software architects.", 
                    Price = 54.99m, 
                    Stock = 60,
                    ImageUrl = "https://via.placeholder.com/300?text=DesignPatterns",
                    Category = "Books", 
                    CompanyId = bookStoreId, 
                    CreatedAt = DateTime.UtcNow 
                }
            };

            context.Products.AddRange(products);

            // Create Users
            var users = new List<User>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@ecommerce.com",
                    DisplayName = "Admin User",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = CORE.Enums.Role.Admin,
                    IsActive = true,
                    IsEmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    AvatarUrl = "https://via.placeholder.com/150?text=Admin"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "user@example.com",
                    DisplayName = "John Doe",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@12345"),
                    Role = CORE.Enums.Role.User,
                    IsActive = true,
                    IsEmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    AvatarUrl = "https://via.placeholder.com/150?text=User"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "jane.smith@example.com",
                    DisplayName = "Jane Smith",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Jane@12345"),
                    Role = CORE.Enums.Role.User,
                    IsActive = true,
                    IsEmailConfirmed = false,
                    CreatedAt = DateTime.UtcNow,
                    AvatarUrl = "https://via.placeholder.com/150?text=Jane"
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
