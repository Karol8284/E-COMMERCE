using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto.ProductDto
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [MinLength(2, ErrorMessage = "Product name must be at least 2 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product description is required")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative")]
        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [MinLength(2, ErrorMessage = "Category must be at least 2 characters")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company ID is required")]
        public Guid CompanyId { get; set; }
    }
}
