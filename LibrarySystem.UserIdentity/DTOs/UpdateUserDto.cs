using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.UserIdentity.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int UserTypeId { get; set; }
    }
}
