using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Reporting.Models
{
    public class CategoryProjection
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}
