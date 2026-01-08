    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    namespace LibrarySystem.Reporting.Models
    {
        public class ProcessedEvent
        {
            [BsonRepresentation(BsonType.String)]

            public Guid EventId { get; set; }
            public DateTime ProcessedAt { get; set; }
        }
    }
