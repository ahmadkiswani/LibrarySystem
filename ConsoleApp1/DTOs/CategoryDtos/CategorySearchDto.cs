using System;
using System.Collections.Generic;
using System.Text;
using LibrarySystem.DTOs;

namespace LibrarySystem.DTOs.CategoryDtos
{
    public class CategorySearchDto : SearchBaseDto
    {
        public int? Number { get; set; }
    }
}
