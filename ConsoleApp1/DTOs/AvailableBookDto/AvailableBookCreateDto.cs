using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.DTOs.AvailableBookDto
{
    public class AvailableBookCreateDto
    {
        [Required]
        public int BookId { get; set; }
    }
}
