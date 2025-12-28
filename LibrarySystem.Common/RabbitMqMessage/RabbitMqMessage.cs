using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Common.Messaging
{

    public class RabbitMqMessage
    {
        public RabbitMqMessage()
        {
            CorrelationId = Guid.NewGuid().ToString();
        }

        public string CorrelationId { get; set; }
    }
}
