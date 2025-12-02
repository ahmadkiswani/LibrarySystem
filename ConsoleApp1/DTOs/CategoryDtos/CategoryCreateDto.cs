using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

