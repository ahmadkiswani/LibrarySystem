using LibrarySystem.Common.Messaging;
using LibrarySystem.LibraryManagement.Helpers;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.UserDtos;
using MassTransit;

public class UserCreatedConsumer : IConsumer<UserCreatedMessage>
{
    private readonly IUserService _userService;

    public UserCreatedConsumer(IUserService userService)
    {
        _userService = userService;
    }

    public async Task Consume(ConsumeContext<UserCreatedMessage> context)
    {
        var msg = context.Message;

        await _userService.ApplyUserCreatedEvent(new UserCreateDto
        {
            ExternalUserId = msg.UserId,
            UserName = msg.UserName,
            UserEmail = msg.Email,
        });
    }
}
