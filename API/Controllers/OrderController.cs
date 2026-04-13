using API.Models.Dto.Order;
using CORE.Entities;
using CORE.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all orders for a user
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders(Guid userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var orderDtos = orders.Select(o => MapOrderToDto(o)).ToList();
            return Ok(orderDtos);
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound($"Order with ID {orderId} not found");

            return Ok(MapOrderToDto(order));
        }

        /// <summary>
        /// Create order from cart or order items
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get or create user - for now, get from query or assume from auth
            // In real app, this would come from JWT token
            var userId = HttpContext.Items["UserId"] as Guid? ?? Guid.Empty;

            // For demonstration, we'll require userId to be passed
            if (userId == Guid.Empty)
                return BadRequest("User ID is required. Please login first.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return NotFound("User not found");

            if (createOrderDto.Items == null || !createOrderDto.Items.Any())
                return BadRequest("Order must contain at least one item");

            // Verify all products exist and have enough stock
            var orderItems = new List<OrderItem>();
            decimal totalPrice = 0;

            foreach (var itemInput in createOrderDto.Items)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == itemInput.ProductId);
                if (product == null)
                    return BadRequest($"Product with ID {itemInput.ProductId} not found");

                if (product.Stock < itemInput.Quantity)
                    return BadRequest($"Insufficient stock for product '{product.Name}'. Available: {product.Stock}");

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = itemInput.ProductId,
                    Quantity = itemInput.Quantity,
                    Price = product.Price
                };
                orderItems.Add(orderItem);
                totalPrice += product.Price * itemInput.Quantity;

                // Decrease stock
                product.Stock -= itemInput.Quantity;
                _context.Products.Update(product);
            }

            // Create order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = OrderStatus.Pending,
                TotalPrice = totalPrice,
                ShippingAddress = createOrderDto.ShippingAddress,
                CreatedAt = DateTime.UtcNow,
                Items = orderItems
            };

            _context.Orders.Add(order);

            // Clear user's cart after successful order
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart != null)
            {
                var cartItems = await _context.CartItems.Where(ci => ci.CartId == cart.Id).ToListAsync();
                _context.CartItems.RemoveRange(cartItems);
                cart.UpdatedAt = DateTime.UtcNow;
                _context.Carts.Update(cart);
            }

            await _context.SaveChangesAsync();

            order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, MapOrderToDto(order));
        }

        /// <summary>
        /// Update order status
        /// </summary>
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                return NotFound("Order not found");

            if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
                return BadRequest($"Invalid order status. Valid values: {string.Join(", ", Enum.GetNames(typeof(OrderStatus)))}");

            order.Status = status;
            if (status == OrderStatus.Cancelled)
                order.CancelledAt = DateTime.UtcNow;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound("Order not found");

            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Shipped)
                return BadRequest("Cannot cancel a shipped or delivered order");

            // Restore product stock
            foreach (var item in order.Items)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                    _context.Products.Update(product);
                }
            }

            order.Status = OrderStatus.Cancelled;
            order.CancelledAt = DateTime.UtcNow;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static OrderDto MapOrderToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status.ToString(),
                TotalPrice = order.TotalPrice,
                ShippingAddress = order.ShippingAddress,
                CreatedAt = order.CreatedAt,
                DeliveryDate = order.DeliveryDate,
                CancelledAt = order.CancelledAt,
                Items = order.Items.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }
    }

    public class UpdateOrderStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }
}
