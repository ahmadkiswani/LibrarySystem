using LibrarySystem.Common.Messaging;
using LibrarySystem.Domain.Data; 
using LibrarySystem.Entities.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

public class UserCreatedConsumer : IConsumer<UserCreatedMessage>
{
    private readonly LibraryDbContext _db;

    public UserCreatedConsumer(LibraryDbContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<UserCreatedMessage> context)
    {
        var msg = context.Message;

        var exists = await _db.Users
            .AnyAsync(u => u.ExternalUserId == msg.UserId);

        if (exists)
            return;

        var user = new User
        {
            ExternalUserId = msg.UserId,
            UserName = msg.UserName,
            UserEmail = msg.Email,
            UserTypeId = msg.UserTypeId
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        Console.WriteLine($"✅ User mirrored in Library DB: {user.UserName}");
    }
}
