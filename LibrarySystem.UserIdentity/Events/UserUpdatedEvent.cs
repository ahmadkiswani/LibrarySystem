namespace LibrarySystem.UserIdentity.Messaging.Events;

public record UserUpdatedEvent(
    int UserId,
    string UserName,
    string Email,
    int UserTypeId,
    DateTime OccurredAt
);
