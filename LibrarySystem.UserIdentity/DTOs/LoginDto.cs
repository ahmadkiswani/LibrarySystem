using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.UserIdentity.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username or Email is required")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
