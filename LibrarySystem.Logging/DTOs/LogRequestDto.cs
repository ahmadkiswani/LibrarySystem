using System;
using System.Collections.Generic;
using System.Text;

public class LogRequestDto
{
    public string CorrelationId { get; set; }
    public DateTime Time { get; set; }
    public string ServiceName { get; set; }
    public string Request { get; set; }
}

