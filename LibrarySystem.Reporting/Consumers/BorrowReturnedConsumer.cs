using LibrarySystem.Common.Events;
using LibrarySystem.Reporting.Models;
using MassTransit;
using MongoDB.Driver;

namespace LibrarySystem.Reporting.Consumers;

public class BorrowReturnedConsumer : IConsumer<BorrowReturnedEvent>
{
    private readonly IMongoCollection<BorrowStatusStats> _stats;
    private readonly IMongoCollection<ProcessedEvent> _processed;

    public BorrowReturnedConsumer(IMongoDatabase db)
    {
        _stats = db.GetCollection<BorrowStatusStats>("BorrowStatusStats");
        _processed = db.GetCollection<ProcessedEvent>("ProcessedEvents");
    }

    public async Task Consume(ConsumeContext<BorrowReturnedEvent> ctx)
    {
        var e = ctx.Message;

        var exists = await _processed
            .Find(x => x.EventId == e.EventId)
            .AnyAsync();

        if (exists)
            return;

        var day = e.OccurredAt.ToString("yyyy-MM-dd");
        var month = e.OccurredAt.ToString("yyyy-MM");

        await _stats.UpdateOneAsync(
            x => x.Period == "daily" && x.Key == day,
            Builders<BorrowStatusStats>.Update
                .Inc(x => x.Returned, 1),
            new UpdateOptions { IsUpsert = true });

        await _stats.UpdateOneAsync(
            x => x.Period == "daily" && x.Key == day && x.Active > 0,
            Builders<BorrowStatusStats>.Update
                .Inc(x => x.Active, -1)
        );

        await _stats.UpdateOneAsync(
            x => x.Period == "monthly" && x.Key == month,
            Builders<BorrowStatusStats>.Update
                .Inc(x => x.Returned, 1),
            new UpdateOptions { IsUpsert = true });

        await _stats.UpdateOneAsync(
            x => x.Period == "monthly" && x.Key == month && x.Active > 0,
            Builders<BorrowStatusStats>.Update
                .Inc(x => x.Active, -1)
        );

        await _processed.InsertOneAsync(new ProcessedEvent
        {
            EventId = e.EventId,
            ProcessedAt = DateTime.UtcNow
        });
    }
}
