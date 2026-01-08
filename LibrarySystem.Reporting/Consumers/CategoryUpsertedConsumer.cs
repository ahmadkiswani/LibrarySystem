using LibrarySystem.Common.Events;
using LibrarySystem.Reporting.Models;
using MassTransit;
using MongoDB.Driver;

namespace LibrarySystem.Reporting.Consumers;

public class CategoryUpsertedConsumer : IConsumer<CategoryUpsertedEvent>
{
    private readonly IMongoCollection<CategoryProjection> _categories;
    private readonly IMongoCollection<DailyBorrowedByCategory> _daily;

    public CategoryUpsertedConsumer(IMongoDatabase db)
    {
        _categories = db.GetCollection<CategoryProjection>("Categories");
        _daily = db.GetCollection<DailyBorrowedByCategory>("DailyBorrowedByCategory");
    }

    public async Task Consume(ConsumeContext<CategoryUpsertedEvent> ctx)
    {
        var e = ctx.Message;

        await _categories.UpdateOneAsync(
            x => x.CategoryId == e.CategoryId,
            Builders<CategoryProjection>.Update
                .Set(x => x.Name, e.CategoryName)
                .Set(x => x.UpdatedAt, DateTime.UtcNow),
            new UpdateOptions { IsUpsert = true });

        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

        await _daily.UpdateManyAsync(
            x => x.Date == today && x.CategoryId == e.CategoryId,
            Builders<DailyBorrowedByCategory>.Update.Set(x => x.CategoryName, e.CategoryName)
        );
    }
}
