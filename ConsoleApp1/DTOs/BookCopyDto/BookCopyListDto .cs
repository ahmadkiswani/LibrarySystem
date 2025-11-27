using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.DTOs.AvailableBookDto
{
    public class BookCopyListDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
