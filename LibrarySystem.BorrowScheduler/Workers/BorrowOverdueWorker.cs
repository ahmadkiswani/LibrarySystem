using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.BorrowScheduler.Workers;

public class BorrowOverdueWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BorrowOverdueWorker> _logger;

    public BorrowOverdueWorker(IServiceScopeFactory scopeFactory, ILogger<BorrowOverdueWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BorrowOverdueWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var borrowRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Borrow>>();
            var borrowService = scope.ServiceProvider.GetRequiredService<IBorrowService>();

            var now = DateTime.UtcNow;

            var overdueBorrowIds = await borrowRepo.GetQueryable()
                .Where(b => b.Status == BorrowStatus.Borrowed && b.DueDate < now)
                .Select(b => b.Id)
                .ToListAsync(stoppingToken);

            _logger.LogInformation("Found {Count} overdue borrows at {Time}", overdueBorrowIds.Count, now);

            foreach (var id in overdueBorrowIds)
            {
             
                await borrowService.MarkOverdueAsync(id);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
