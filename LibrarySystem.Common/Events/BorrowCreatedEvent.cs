namespace LibrarySystem.Common.Events;

public record BorrowCreatedEvent
{
    public Guid EventId { get; init; }
    public DateTime OccurredAt { get; init; }

    public int BorrowId { get; init; }
    public int UserId { get; init; }
    public string UserName { get; init; } = string.Empty;

    public int BookCopyId { get; init; }

    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
}
