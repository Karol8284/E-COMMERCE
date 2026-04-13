using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto.Order
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Shipping address is required")]
        [MinLength(5, ErrorMessage = "Shipping address must be at least 5 characters")]
        public string ShippingAddress { get; set; } = string.Empty;

        public List<OrderItemInputDto> Items { get; set; } = new();
    }

    public class OrderItemInputDto
    {
        [Required(ErrorMessage = "Product ID is required")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
