using LibrarySystem.Common.Events;
using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.BorrowDTOs;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Services;

public class BorrowService : IBorrowService
{
    private readonly IGenericRepository<Borrow> _borrowRepo;
    private readonly IGenericRepository<BookCopy> _copyRepo;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IPublishEndpoint _publish;

    public BorrowService(
        IGenericRepository<Borrow> borrowRepo,
        IGenericRepository<BookCopy> copyRepo,
        IGenericRepository<User> userRepo,
        IPublishEndpoint publish)
    {
        _borrowRepo = borrowRepo;
        _copyRepo = copyRepo;
        _userRepo = userRepo;
        _publish = publish;
    }

    public async Task BorrowBook(BorrowCreateDto dto)
    {
        using var transaction = await _borrowRepo.Context.Database.BeginTransactionAsync();

        try
        {
            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new Exception("User not found");
            if (user.IsDeleted)
                throw new Exception("User is not active");

            // ✅ جلب Book + Category
            var copy = await _copyRepo.GetQueryable()
                .Include(c => c.Book)
                    .ThenInclude(b => b.Category)
                .FirstOrDefaultAsync(c => c.Id == dto.BookCopyId);

            if (copy == null)
                throw new Exception("Copy not found");
            if (!copy.IsAvailable)
                throw new Exception("Copy is not available");
            if (copy.Book == null)
                throw new Exception("Book not found for this copy");
            if (copy.Book.Category == null)
                throw new Exception("Category not found for this book");

            int activeBorrows = await _borrowRepo.GetQueryable()
                .CountAsync(b =>
                    b.UserId == dto.UserId &&
                    b.Status == BorrowStatus.Borrowed
                );

            if (activeBorrows >= 5)
                throw new Exception("User cannot borrow more than 5 books at once");

            copy.IsAvailable = false;
            await _copyRepo.UpdateAsync(copy);

            var now = DateTime.UtcNow;

            var borrow = new Borrow
            {
                UserId = dto.UserId,
                BookCopyId = dto.BookCopyId,
                BorrowDate = now,
                DueDate = now.AddDays(5),
                Status = BorrowStatus.Borrowed,
                OverdueDays = 0
            };

            await _borrowRepo.AddAsync(borrow);
            await _borrowRepo.SaveAsync();

            await transaction.CommitAsync();

            await _publish.Publish(new BorrowCreatedEvent
            {
                EventId = Guid.NewGuid(),
                OccurredAt = now,

                BorrowId = borrow.Id,
                UserId = borrow.UserId,
                BookCopyId = borrow.BookCopyId,
                UserName = user.UserName,
                CategoryId = copy.Book.CategoryId,
                CategoryName = copy.Book.Category.Name
            }, ctx => ctx.SetRoutingKey("borrow.created"));
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task ReturnBook(BorrowReturnDto dto)
    {
        using var transaction = await _borrowRepo.Context.Database.BeginTransactionAsync();

        try
        {
            var borrow = await _borrowRepo.GetByIdAsync(dto.Id);
            if (borrow == null)
                throw new Exception("Borrow record not found");

            if (borrow.Status == BorrowStatus.Returned)
                throw new Exception("Already returned");

            var wasOverdue = borrow.Status == BorrowStatus.Overdue;
            var now = DateTime.UtcNow;

            borrow.ReturnDate = now;
            borrow.Status = BorrowStatus.Returned;
            borrow.OverdueDays = 0;

            await _borrowRepo.UpdateAsync(borrow);

            var copy = await _copyRepo.GetByIdAsync(borrow.BookCopyId);
            if (copy != null)
            {
                copy.IsAvailable = true;
                await _copyRepo.UpdateAsync(copy);
            }

            await _borrowRepo.SaveAsync();
            await transaction.CommitAsync();

            await _publish.Publish(new BorrowReturnedEvent
            {
                EventId = Guid.NewGuid(),
                OccurredAt = now,

                BorrowId = borrow.Id,
                UserId = borrow.UserId,

                ReturnedAt = now,
                WasOverdue = wasOverdue
            }, ctx => ctx.SetRoutingKey("borrow.returned"));
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task MarkOverdueAsync(int borrowId)
    {
        using var transaction = await _borrowRepo.Context.Database.BeginTransactionAsync();

        try
        {
            var borrow = await _borrowRepo.GetByIdAsync(borrowId);
            if (borrow == null)
                return;

            if (borrow.Status != BorrowStatus.Borrowed)
                return;

            var now = DateTime.UtcNow;

            if (borrow.DueDate >= now)
                return;

            borrow.Status = BorrowStatus.Overdue;
            borrow.OverdueDays = (now - borrow.DueDate).Days;

            await _borrowRepo.UpdateAsync(borrow);
            await _borrowRepo.SaveAsync();
            await transaction.CommitAsync();

            await _publish.Publish(new BorrowOverdueEvent
            {
                EventId = Guid.NewGuid(),
                OccurredAt = now,

                BorrowId = borrow.Id,
                UserId = borrow.UserId,
                DaysOverdue = borrow.OverdueDays ?? 0
            }, ctx => ctx.SetRoutingKey("borrow.overdue"));
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Borrow>> Search(BorrowSearchDto dto)
    {
        var query = _borrowRepo.GetQueryable()
            .Where(b => !dto.Number.HasValue || b.Id == dto.Number.Value)
            .Where(b => !dto.UserId.HasValue || b.UserId == dto.UserId.Value)
            .Where(b => !dto.BookCopyId.HasValue || b.BookCopyId == dto.BookCopyId.Value)
            .Where(b =>
                !dto.Returned.HasValue ||
                (dto.Returned.Value
                    ? b.Status == BorrowStatus.Returned
                    : b.Status != BorrowStatus.Returned)
            );

        int page = dto.Page > 0 ? dto.Page : 1;
        int pageSize = dto.PageSize > 0 ? dto.PageSize : 10;

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
