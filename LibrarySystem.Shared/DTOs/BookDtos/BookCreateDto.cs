using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.Shared.DTOs.BookDtos
{
    public class BookCreateDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Title { get; set; }= string.Empty;

        [Required]
        [MaxLength(20)]
        public string Version { get; set; }= string.Empty;
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PublisherId { get; set; }

    }

}
