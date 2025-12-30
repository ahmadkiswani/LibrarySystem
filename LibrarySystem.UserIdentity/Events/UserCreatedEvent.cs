namespace LibrarySystem.UserIdentity.Messaging;

public record UserCreatedEvent(
    int UserId,
    string UserName,
    string Email,
    int UserTypeId,
    DateTime OccurredAt
);
