using System;
using System.Collections.Generic;
using System.Text;
 namespace LibrarySystem.Shared.DTOs.BookDtos
{
    public class BookSearchDto:SearchBaseDto
    {
        public string? Title { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? Version { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public int? PublisherId { get; set; }
        public bool? Available { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;

    }

}
