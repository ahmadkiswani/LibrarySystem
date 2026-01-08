namespace LibrarySystem.Common.Events;

public record CategoryUpsertedEvent
{
    public Guid EventId { get; init; }
    public DateTime OccurredAt { get; init; }

    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
}
