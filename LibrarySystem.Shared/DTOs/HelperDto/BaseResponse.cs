using System;
using System.Collections.Generic;
using System.Text;


namespace LibrarySystem.Shared.DTOs.HelperDto
{
    public class BaseResponse<T>
    {
    
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string? CorrelationId { get; set; }

    }
}

