using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.BorrowDTOs;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IGenericRepository<Borrow> _borrowRepo;
        private readonly IGenericRepository<BookCopy> _copyRepo;
        private readonly IGenericRepository<User> _userRepo;

        public BorrowService(
            IGenericRepository<Borrow> borrowRepo,
            IGenericRepository<BookCopy> copyRepo,
            IGenericRepository<User> userRepo)
        {
            _borrowRepo = borrowRepo;
            _copyRepo = copyRepo;
            _userRepo = userRepo;
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

                var copy = await _copyRepo.GetByIdAsync(dto.BookCopyId);
                if (copy == null)
                    throw new Exception("Copy not found");

                if (!copy.IsAvailable)
                    throw new Exception("Copy is not available");

                int activeBorrows = await _borrowRepo.Query()
                    .CountAsync(b => b.UserId == dto.UserId && b.ReturnDate == null);

                if (activeBorrows >= 5)
                    throw new Exception("User cannot borrow more than 5 books at once");

                copy.IsAvailable = false;
                await _copyRepo.UpdateAsync(copy);

                var borrow = new Borrow
                {
                    UserId = dto.UserId,
                    BookCopyId = dto.BookCopyId,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(5)
                };

                await _borrowRepo.AddAsync(borrow);
                await _borrowRepo.SaveAsync();

                await transaction.CommitAsync();
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

                if (borrow.ReturnDate != null)
                    throw new Exception("Already returned");

                borrow.ReturnDate = DateTime.Now;
                await _borrowRepo.UpdateAsync(borrow);

                var copy = await _copyRepo.GetByIdAsync(borrow.BookCopyId);
                if (copy != null)
                {
                    copy.IsAvailable = true;
                    await _copyRepo.UpdateAsync(copy);
                }

                await _borrowRepo.SaveAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<List<Borrow>> Search(BorrowSearchDto dto)
        {
            var query = _borrowRepo.Query()
                .Where(b => !dto.Number.HasValue || b.Id == dto.Number.Value)
                .Where(b => !dto.UserId.HasValue || b.UserId == dto.UserId.Value)
                .Where(b => !dto.BookCopyId.HasValue || b.BookCopyId == dto.BookCopyId.Value)
                .Where(b =>
                    !dto.Returned.HasValue ||
                    (dto.Returned.Value ? b.ReturnDate != null : b.ReturnDate == null)
                );

            int page = dto.Page > 0 ? dto.Page : 1;
            int pageSize = dto.PageSize > 0 ? dto.PageSize : 10;

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
