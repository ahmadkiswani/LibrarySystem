using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.DTOs.BookDtos
{
    public class BookUpdateDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Version { get; set; }

        [Required]
        [Range(1, 2000)]
        public int TotalCopies { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int PublisherId { get; set; }
    }

}
