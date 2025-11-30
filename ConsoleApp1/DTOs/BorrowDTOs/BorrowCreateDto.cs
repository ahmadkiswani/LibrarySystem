using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.DTOs.BorrowDTOs
{
    public class BorrowCreateDto
    {
        [Required]  
        public int Id { get; set; }
        [Required]
        public int BookCopyId { get; set; }
        
      


    }


}
