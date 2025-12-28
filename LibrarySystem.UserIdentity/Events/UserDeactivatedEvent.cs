namespace LibrarySystem.UserIdentity.Messaging.Events;

public record UserDeactivatedEvent(
    int UserId,
    DateTime OccurredAt
);
