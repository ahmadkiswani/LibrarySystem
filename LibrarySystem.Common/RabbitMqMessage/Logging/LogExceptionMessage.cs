

namespace LibrarySystem.Common.Messaging
{
    public class LogExceptionMessage : RabbitMqMessage
    {
        public DateTime Time { get; set; }
        public string ServiceName { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? StackTrace { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string ErrorType { get; set; } = "Exception";
    }
}