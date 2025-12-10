
using System.Text.Json.Serialization;

namespace LibrarySystem.Shared.DTOs.BookDtos
{
    public class BookSearchDto
    {
        public string? Title { get; set; } 
        public int? AuthorId { get; set; } 
        public int? CategoryId { get; set; } 
        public int? PublisherId { get; set; } 
        [JsonIgnore]
        public int? Page { get; set; } 
        [JsonIgnore]
        public int? PageSize { get; set; } 



    }

}
