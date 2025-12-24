

namespace LibrarySystem.Logging.DTOs
{
    public class LogExceptionDto
    {
     public required string CorrelationId { get; set; }
    public DateTime Time { get; set; }
    public string? ServiceName { get; set; }


    public string? Request { get; set; }
    public string? Response { get; set; }

    public string? Message { get; set; }
    public string? StackTrace { get; set; }
    }
}
