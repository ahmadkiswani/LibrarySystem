using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.Shared.DTOs.BorrowDTOs
{
    public class BorrowCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookCopyId { get; set; }
    }

}



