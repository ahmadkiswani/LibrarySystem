using System;

namespace LibrarySystem.Common.Messaging
{
    public class LogRequestMessage : RabbitMqMessage
    {
        public DateTime Time { get; set; }
        public string ServiceName { get; set; } = null!;
        public string Request { get; set; } = null!;
    }
}
