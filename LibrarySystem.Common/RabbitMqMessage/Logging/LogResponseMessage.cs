using System;

namespace LibrarySystem.Common.Messaging
{ 
    public class LogResponseMessage : RabbitMqMessage
    {
        public DateTime Time { get; set; }
        public string ServiceName { get; set; } = null!;
        public string Response { get; set; } = null!;
    }
}
