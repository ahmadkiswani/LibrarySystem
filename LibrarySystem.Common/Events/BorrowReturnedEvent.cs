namespace LibrarySystem.Common.Events;

public record BorrowReturnedEvent
{
    public Guid EventId { get; init; }
    public DateTime OccurredAt { get; init; }

    public int BorrowId { get; init; }
    public int UserId { get; init; }

    public DateTime ReturnedAt { get; init; }
    public bool WasOverdue { get; init; }
}
