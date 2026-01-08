using LibrarySystem.Common.Events;
using LibrarySystem.Reporting.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static MassTransit.Transports.ReceiveEndpoint;

namespace LibrarySystem.Reporting.Consumers;

public class BorrowReturnedConsumer : IConsumer<BorrowReturnedEvent>
{
    private readonly IMongoCollection<BorrowStatusStats> _stats;
    private readonly IMongoCollection<ProcessedEvent> _processed;
    private readonly ILogger<BorrowReturnedConsumer> _logger;


    public BorrowReturnedConsumer(IMongoDatabase db, ILogger<BorrowReturnedConsumer> logger)
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

        var dailyStat = await _stats
            .Find(x => x.Period == "daily" && x.Key == day)
            .FirstOrDefaultAsync();

        if (dailyStat != null && dailyStat.Active > 0)
        {
            await _stats.UpdateOneAsync(
                x => x.Period == "daily" && x.Key == day,
                Builders<BorrowStatusStats>.Update
                    .Inc(x => x.Returned, 1)
                    .Inc(x => x.Active, -1),
                new UpdateOptions { IsUpsert = true });
        }
        else
        {
            if (dailyStat != null && dailyStat.Active <= 0)
            {
                _logger.LogWarning(
                    "Daily return received but Active already zero. BorrowId={BorrowId}",
                    e.BorrowId);
            }

            await _stats.UpdateOneAsync(
                x => x.Period == "daily" && x.Key == day,
                Builders<BorrowStatusStats>.Update
                    .Inc(x => x.Returned, 1),
                new UpdateOptions { IsUpsert = true });
        }

        var monthlyStat = await _stats
            .Find(x => x.Period == "monthly" && x.Key == month)
            .FirstOrDefaultAsync();

        if (monthlyStat != null && monthlyStat.Active > 0)
        {
            await _stats.UpdateOneAsync(
                x => x.Period == "monthly" && x.Key == month,
                Builders<BorrowStatusStats>.Update
                    .Inc(x => x.Returned, 1)
                    .Inc(x => x.Active, -1),
                new UpdateOptions { IsUpsert = true });
        }
        else
        {
            if (monthlyStat != null && monthlyStat.Active <= 0)
            {
                _logger.LogWarning(
                    "Monthly return received but Active already zero. BorrowId={BorrowId}",
                    e.BorrowId);
            }

            await _stats.UpdateOneAsync(
                x => x.Period == "monthly" && x.Key == month,
                Builders<BorrowStatusStats>.Update
                    .Inc(x => x.Returned, 1),
                new UpdateOptions { IsUpsert = true });
        }

        await _processed.InsertOneAsync(new ProcessedEvent
        {
            EventId = e.EventId,
            ProcessedAt = DateTime.UtcNow
        });
    }

}