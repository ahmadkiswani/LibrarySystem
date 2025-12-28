using LibrarySystem.Common.Messaging;
using LibrarySystem.Domain.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

public class UserDeactivatedConsumer : IConsumer<UserDeactivatedMessage>
{
    private readonly LibraryDbContext _db;
    public UserDeactivatedConsumer(LibraryDbContext db) => _db = db;

    public async Task Consume(ConsumeContext<UserDeactivatedMessage> context)
    {
        var msg = context.Message;

        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.ExternalUserId == msg.UserId);

        if (user == null)
            return;

        user.IsDeleted = true;          
        user.DeletedDate = msg.OccurredAt; 

        await _db.SaveChangesAsync();

        Console.WriteLine($"🛑 User deactivated: {user.UserName}");
    }
}
