using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.UserIdentity.DTOs
{
    public class RegisterDto
    {
        [Required, EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = string.Empty;

    }
}
