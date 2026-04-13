using API.Models.Dto.ProductDto;
using CORE.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all products with optional filtering and pagination
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(
            [FromQuery] string? category = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.ToLower() == category.ToLower());

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Company)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    Category = p.Category,
                    CompanyId = p.CompanyId,
                    CompanyName = p.Company.Name,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Company)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound($"Product with ID {id} not found");

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                Category = product.Category,
                CompanyId = product.CompanyId,
                CompanyName = product.Company.Name,
                CreatedAt = product.CreatedAt
            };

            return Ok(productDto);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verify company exists
            var companyExists = await _context.Companies.AnyAsync(c => c.Id == createDto.CompanyId);
            if (!companyExists)
                return BadRequest($"Company with ID {createDto.CompanyId} not found");

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                Stock = createDto.Stock,
                ImageUrl = createDto.ImageUrl,
                Category = createDto.Category,
                CompanyId = createDto.CompanyId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == product.CompanyId);

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                Category = product.Category,
                CompanyId = product.CompanyId,
                CompanyName = company?.Name ?? string.Empty,
                CreatedAt = product.CreatedAt
            };

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, productDto);
        }

        /// <summary>
        /// Update product
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] CreateProductDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound($"Product with ID {id} not found");

            product.Name = updateDto.Name;
            product.Description = updateDto.Description;
            product.Price = updateDto.Price;
            product.Stock = updateDto.Stock;
            product.ImageUrl = updateDto.ImageUrl;
            product.Category = updateDto.Category;
            product.CompanyId = updateDto.CompanyId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete product
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound($"Product with ID {id} not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
