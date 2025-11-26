using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.DTOs.BorrowDTOs
{
    public class BorrowCreateDto
    {
        [Required]  
        public int UserId { get; set; }
        [Required]
        public int AvailableBookId { get; set; }
        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }


    }


}
