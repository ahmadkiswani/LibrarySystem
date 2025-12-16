using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Logging.DTOs
{
    public class LogResponseDto
    {
        public string CorrelationId { get; set; }
        public string ServiceName { get; set; }
        public string Response { get; set; }
    }
}
