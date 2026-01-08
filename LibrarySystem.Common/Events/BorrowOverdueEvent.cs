namespace LibrarySystem.Common.Events;

public record BorrowOverdueEvent
{
    public Guid EventId { get; init; }
    public DateTime OccurredAt { get; init; }

    public int BorrowId { get; init; }
    public int UserId { get; init; }
    public string UserName { get; init; }

    public int DaysOverdue { get; init; }
}
    