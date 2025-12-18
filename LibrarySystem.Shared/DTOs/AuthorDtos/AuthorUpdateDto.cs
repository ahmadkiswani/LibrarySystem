using System.ComponentModel.DataAnnotations;
namespace LibrarySystem.Shared.DTOs.AuthorDtos
{
    public class AuthorUpdateDto
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string AuthorName { get; set; }= string.Empty;   
        public int Id { get; set; }

    }
}
