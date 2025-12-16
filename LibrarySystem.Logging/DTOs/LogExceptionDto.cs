using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Logging.DTOs
{
    public class LogExceptionDto
    {
        public string CorrelationId { get; set; }
        public string ServiceName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
