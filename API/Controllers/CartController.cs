using API.Models.Dto.Cart;
using CORE.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user's cart
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDto>> GetCart(Guid userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found");

            var cartDto = MapCartToDto(cart);
            return Ok(cartDto);
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        [HttpPost("{userId}/add")]
        public async Task<ActionResult<CartDto>> AddToCart(Guid userId, [FromBody] AddToCartDto addToCartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found");

            // Check if product exists and has stock
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == addToCartDto.ProductId);
            if (product == null)
                return NotFound($"Product with ID {addToCartDto.ProductId} not found");

            if (product.Stock < addToCartDto.Quantity)
                return BadRequest($"Insufficient stock. Available: {product.Stock}");

            // Check if item already in cart
            var existingItem = cart.Items.FirstOrDefault(ci => ci.ProductId == addToCartDto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += addToCartDto.Quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = addToCartDto.ProductId,
                    Quantity = addToCartDto.Quantity,
                    AddedAt = DateTime.UtcNow
                };
                _context.CartItems.Add(cartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            return Ok(MapCartToDto(cart));
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        [HttpDelete("{userId}/items/{cartItemId}")]
        public async Task<ActionResult<CartDto>> RemoveFromCart(Guid userId, Guid cartItemId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found");

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartItemId);
            if (cartItem == null)
                return NotFound("Cart item not found");

            _context.CartItems.Remove(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            return Ok(MapCartToDto(cart));
        }

        /// <summary>
        /// Clear entire cart
        /// </summary>
        [HttpDelete("{userId}/clear")]
        public async Task<IActionResult> ClearCart(Guid userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found");

            _context.CartItems.RemoveRange(cart.Items);
            cart.UpdatedAt = DateTime.UtcNow;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static CartDto MapCartToDto(Cart cart)
        {
            var items = cart.Items.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                CartId = ci.CartId,
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                ProductPrice = ci.Product.Price,
                Quantity = ci.Quantity,
                AddedAt = ci.AddedAt
            }).ToList();

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = items,
                TotalPrice = items.Sum(i => i.Subtotal)
            };
        }
    }
}
