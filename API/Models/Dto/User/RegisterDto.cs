using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto.User
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Display name is required")]
        [MinLength(2, ErrorMessage = "Display name must be at least 2 characters")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
