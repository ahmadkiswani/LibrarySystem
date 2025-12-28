namespace LibrarySystem.UserIdentity.Messaging.Events;

public record UserCreatedEvent(
    int UserId,
    string UserName,
    string Email,
    int UserTypeId,
    DateTime OccurredAt
);
