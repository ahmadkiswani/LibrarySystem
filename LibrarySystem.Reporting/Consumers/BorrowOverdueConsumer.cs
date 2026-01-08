using LibrarySystem.Common.Events;
using LibrarySystem.Reporting.Models;
using MassTransit;
using MongoDB.Driver;

namespace LibrarySystem.Reporting.Consumers;

public class BorrowOverdueConsumer : IConsumer<BorrowOverdueEvent>
{
    private readonly IMongoCollection<BorrowStatusStats> _stats;
    private readonly IMongoCollection<TopOverdueUsers> _overdueUsers;
    private readonly IMongoCollection<ProcessedEvent> _processed;

    public BorrowOverdueConsumer(IMongoDatabase db)
    {
        _stats = db.GetCollection<BorrowStatusStats>("BorrowStatusStats");
        _overdueUsers = db.GetCollection<TopOverdueUsers>("TopOverdueUsers");
        _processed = db.GetCollection<ProcessedEvent>("ProcessedEvents");
    }

    public async Task Consume(ConsumeContext<BorrowOverdueEvent> ctx)
    {
        var e = ctx.Message;

        var exists = await _processed.Find(x => x.EventId == e.EventId).AnyAsync();
        if (exists)
            return;

        var day = e.OccurredAt.ToString("yyyy-MM-dd");
        var month = e.OccurredAt.ToString("yyyy-MM");

        await _stats.UpdateOneAsync(
            x => x.Period == "daily" && x.Key == day,
            Builders<BorrowStatusStats>.Update.Inc(x => x.Overdue, 1),
            new UpdateOptions { IsUpsert = true });

        await _stats.UpdateOneAsync(
            x => x.Period == "monthly" && x.Key == month,
            Builders<BorrowStatusStats>.Update.Inc(x => x.Overdue, 1),
            new UpdateOptions { IsUpsert = true });

        await _overdueUsers.UpdateOneAsync(
            x => x.UserId == e.UserId,
            Builders<TopOverdueUsers>.Update
                .Inc(x => x.OverdueCount, 1)
                .SetOnInsert(x => x.UserName, e.UserName),
            new UpdateOptions { IsUpsert = true });

        await _processed.InsertOneAsync(new ProcessedEvent
        {
            EventId = e.EventId,
            ProcessedAt = DateTime.UtcNow
        });
    }
}
