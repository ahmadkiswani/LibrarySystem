using LibrarySystem.Common.Messaging;
using LibrarySystem.Domain.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;


public class UserUpdatedConsumer : IConsumer<UserUpdatedMessage>
{
    private readonly LibraryDbContext _db;

    public UserUpdatedConsumer(LibraryDbContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<UserUpdatedMessage> context)
    {
        var msg = context.Message;

        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.ExternalUserId == msg.UserId);

        if (user == null)
            return;

        user.UserName = msg.UserName;
        user.UserEmail = msg.Email;
        user.UserTypeId = msg.UserTypeId;

        await _db.SaveChangesAsync();

    }
}
