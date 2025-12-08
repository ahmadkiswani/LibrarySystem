using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Shared.DTOs.Helper
{
    public class ValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
