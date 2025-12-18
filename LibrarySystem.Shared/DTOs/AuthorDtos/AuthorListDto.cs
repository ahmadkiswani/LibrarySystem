using System.ComponentModel.DataAnnotations;
namespace LibrarySystem.Shared.DTOs
{
    public class AuthorListDto
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string AuthorName { get; set; }= string.Empty;   
    }
}

