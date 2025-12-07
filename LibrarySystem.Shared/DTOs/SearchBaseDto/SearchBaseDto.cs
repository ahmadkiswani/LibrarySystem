using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Shared.DTOs
{
    public class SearchBaseDto
    {
        public string? Text { get; set; }

        public string? Sort { get; set; }  


        public int Page { get; set; } 
        public int PageSize { get; set; } 
    }
}
