using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
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
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var borrowRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Borrow>>();

            var today = DateTime.UtcNow.Date;

            var toOverdue = await borrowRepo.GetQueryable()
                .Where(b =>
                    b.Status == BorrowStatus.Borrowed &&
                    b.DueDate.Date < today
                )
                .ToListAsync(stoppingToken);

            foreach (var borrow in toOverdue)
            {
                borrow.Status = BorrowStatus.Overdue;
                borrow.OverdueDays = (today - borrow.DueDate.Date).Days;

                await borrowRepo.UpdateAsync(borrow);
            }

            var overdueBorrows = await borrowRepo.GetQueryable()
                .Where(b => b.Status == BorrowStatus.Overdue)
                .ToListAsync(stoppingToken);

            foreach (var borrow in overdueBorrows)
            {
                var days = (today - borrow.DueDate.Date).Days;
                if (days < 0) days = 0;

                if (borrow.OverdueDays != days)
                {
                    borrow.OverdueDays = days;
                    await borrowRepo.UpdateAsync(borrow);
                }
            }

            await borrowRepo.SaveAsync();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
