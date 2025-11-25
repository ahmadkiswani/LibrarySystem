using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.DTOs
{

    public class PublisherUpdateDto
    {
        [Required]
        [StringLength(80)]
        public string Name { get; set; }
    }
}

