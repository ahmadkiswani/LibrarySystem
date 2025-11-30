using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.DTOs
{
    public class SearchBaseDto
    {
        public string? Text { get; set; }

        public string? Sort { get; set; }  // asc, desc


        public int Page { get; set; } 
        public int PageSize { get; set; } 
    }
}
