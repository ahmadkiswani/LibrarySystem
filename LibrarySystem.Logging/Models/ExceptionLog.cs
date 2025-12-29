using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LibrarySystem.Logging.Models
{
    public class ExceptionLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string CorrelationId { get; set; } = null!;

        public DateTime Time { get; set; }

        public string? ServiceName { get; set; } = null!;

        public string? Message { get; set; } = null!;

        public string?Request { get;  set; }
        public string? Response { get;  set; }
    }
}
