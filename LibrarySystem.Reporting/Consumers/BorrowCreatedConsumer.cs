using LibrarySystem.Common.Events;
using LibrarySystem.Reporting.Models;
using MassTransit;
using MongoDB.Driver;

namespace LibrarySystem.Reporting.Consumers;

public class BorrowCreatedConsumer : IConsumer<BorrowCreatedEvent>
{
    private readonly IMongoCollection<MonthlyTopActiveUsers> _monthly;
    private readonly IMongoCollection<DailyBorrowedByCategory> _daily;
    private readonly IMongoCollection<BorrowStatusStats> _stats;
    private readonly IMongoCollection<ProcessedEvent> _processed;

    public BorrowCreatedConsumer(IMongoDatabase db)
    {
        _monthly = db.GetCollection<MonthlyTopActiveUsers>("MonthlyTopActiveUsers");
        _daily = db.GetCollection<DailyBorrowedByCategory>("DailyBorrowedByCategory");
        _stats = db.GetCollection<BorrowStatusStats>("BorrowStatusStats");
        _processed = db.GetCollection<ProcessedEvent>("ProcessedEvents");
    }

    public async Task Consume(ConsumeContext<BorrowCreatedEvent> ctx)
    {
        var e = ctx.Message;

        var exists = await _processed.Find(x => x.EventId == e.EventId).AnyAsync();
        if (exists) return;

        var day = e.OccurredAt.ToString("yyyy-MM-dd");
        var month = e.OccurredAt.ToString("yyyy-MM");

        await _daily.UpdateOneAsync(
            x => x.Date == day && x.CategoryId == e.CategoryId,
            Builders<DailyBorrowedByCategory>.Update
                .Inc(x => x.BorrowCount, 1)
                .Set(x => x.CategoryName, e.CategoryName),
            new UpdateOptions { IsUpsert = true });

        await _monthly.UpdateOneAsync(
            x => x.Month == month && x.UserId == e.UserId,
            Builders<MonthlyTopActiveUsers>.Update
                .Inc(x => x.BorrowCount, 1)
                .SetOnInsert(x => x.UserName, e.UserName),
            new UpdateOptions { IsUpsert = true });

        await _stats.UpdateOneAsync(
            x => x.Period == "daily" && x.Key == day,
            Builders<BorrowStatusStats>.Update.Inc(x => x.Active, 1),
            new UpdateOptions { IsUpsert = true });

        await _processed.InsertOneAsync(new ProcessedEvent
        {
            EventId = e.EventId,
            ProcessedAt = DateTime.UtcNow
        });
    }
}
