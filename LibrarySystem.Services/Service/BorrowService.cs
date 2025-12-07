using LibrarySystem.Domain.Data;
using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.BorrowDTOs;

namespace LibrarySystem.Services
{
    public class BorrowService: IBorrowService
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
            var copy = await _copyRepo.GetByIdAsync(dto.BookCopyId);
            if (copy == null || copy.IsDeleted)
                throw new Exception("Copy not found");

            if (!copy.IsAvailable)
                throw new Exception("Copy not available");

            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user == null || user.IsDeleted)
                throw new Exception("User not found");

            copy.IsAvailable = false;
            copy.LastModifiedDate = DateTime.Now;
            copy.LastModifiedBy = dto.UserId;
            await _copyRepo.Update(copy);

            var borrow = new Borrow
            {
                UserId = dto.UserId,
                BookCopyId = dto.BookCopyId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(5),
                CreatedBy = dto.UserId,
                CreatedDate = DateTime.Now
            };

            await _borrowRepo.AddAsync(borrow);
            await _borrowRepo.SaveAsync();
        }

 
        public async Task ReturnBook(BorrowReturnDto dto)
        {
          
            var borrow = await _borrowRepo.GetByIdAsync(dto.Id);
            if (borrow == null)
                throw new Exception("Borrow record not found");

            if (borrow.ReturnDate != null)
                throw new Exception("Already returned");

            borrow.ReturnDate = DateTime.Now;
            borrow.LastModifiedBy = 1;
            borrow.LastModifiedDate = DateTime.Now;

            await _borrowRepo.Update(borrow);

            var copy = await _copyRepo.GetByIdAsync(borrow.BookCopyId);
            if (copy != null)
            {
                copy.IsAvailable = true;
                copy.LastModifiedBy = 1;
                copy.LastModifiedDate = DateTime.Now;
                await _copyRepo.Update(copy);
            }

            await _borrowRepo.SaveAsync();
        }
        public async Task<List<Borrow>> Search(BorrowSearchDto dto)
        {
            IQueryable<Borrow> query =
                (await _borrowRepo.FindAsync(b => true)).AsQueryable();

            query = query
                .Where(b => !dto.Number.HasValue || b.Id == dto.Number.Value)
                .Where(b => !dto.UserId.HasValue || b.UserId == dto.UserId.Value)
                .Where(b => !dto.BookCopyId.HasValue || b.BookCopyId == dto.BookCopyId.Value)
                .Where(b => !dto.Returned.HasValue ||
                        (dto.Returned.Value == true ? b.ReturnDate != null
                                                     : b.ReturnDate == null));

            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 ? 10 : dto.PageSize;

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


    }
}
    