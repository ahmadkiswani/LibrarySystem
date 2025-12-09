using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.Shared.DTOs
{

    public class PublisherUpdateDto
    {
        public int Id { get; set; } 
        [Required]
        [StringLength(80)]
        public string Name { get; set; }= string.Empty;     
    }
}

