using LibrarySystem.DTOs.BorrowDTOs;
using LibrarySystem.Models;
using LibrarySystem.Repository;

namespace LibrarySystem.Service
{
    public class BorrowService
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
            if (copy == null || copy.IsDeleted || !copy.IsAvailable)
                throw new Exception("Copy not available");

            var user = await _userRepo.GetByIdAsync(dto.Id);
            if (user == null || user.IsDeleted)
                throw new Exception("User not found");

            copy.IsAvailable = false;
            copy.LastModifiedDate = DateTime.Now;
            copy.LastModifiedBy = dto.Id;
            await _copyRepo.Update(copy);

            var borrow = new Borrow
            {
                UserId = dto.Id,
                BookCopyId = dto.BookCopyId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(5),
                CreatedBy = 0,
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
    }
}
