namespace LibrarySystem.Contracts.Common
{
    public abstract class EventBase
    {
        public Guid EventId { get; set; } = Guid.NewGuid();

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        public string? CorrelationId { get; set; }
    }
}
