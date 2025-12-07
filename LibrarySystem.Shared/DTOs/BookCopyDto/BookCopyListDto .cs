using System;
using System.Collections.Generic;
using System.Text;
using LibrarySystem.Entities.Models;
namespace LibrarySystem.Shared.DTOs .AvailableBookDto
{
    public class BookCopyListDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
