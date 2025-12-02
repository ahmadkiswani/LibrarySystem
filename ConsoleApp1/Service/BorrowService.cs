using LibrarySystem.Data;
using LibrarySystem.DTOs.BorrowDTOs;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class BorrowService
    {
        private readonly LibraryDbContext _context;

        public BorrowService(LibraryDbContext context)
        {
            _context = context;
        }

        public void BorrowBook(BorrowCreateDto dto)
        {
            var copy = _context.BookCopies
                .FirstOrDefault(x => x.Id == dto.BookCopyId && !x.IsDeleted);

            if (copy == null || !copy.IsAvailable)
                throw new Exception("This copy is not available");

            var user = _context.Users.FirstOrDefault(u => u.Id == dto.Id);

            if (user == null)
                throw new Exception("User does not exist");

            copy.IsAvailable = false;
            copy.LastModifiedBy = 1;
            copy.LastModifiedDate = DateTime.Now;

            Borrow borrow = new Borrow
            {
                UserId = dto.Id,
                BookCopyId = dto.BookCopyId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(5),
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            _context.Borrows.Add(borrow);
            _context.SaveChanges();
        }

        public void ReturnBook(BorrowReturnDto dto)
        {
            var borrow = _context.Borrows.FirstOrDefault(z => z.Id == dto.Id);

            if (borrow == null)
                throw new Exception("Borrow record not found");

            if (borrow.ReturnDate != null)
                throw new Exception("This book is already returned");

            borrow.ReturnDate = DateTime.Now;
            borrow.LastModifiedBy = 1;
            borrow.LastModifiedDate = DateTime.Now;

            var copy = _context.BookCopies.FirstOrDefault(x => x.Id == borrow.BookCopyId);

            if (copy != null)
            {
                copy.IsAvailable = true;
                copy.LastModifiedBy = 1;
                copy.LastModifiedDate = DateTime.Now;
            }

            _context.SaveChanges();
        }

        public List<Borrow> GetBorrowedBooksByUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            return _context.Borrows
                .Where(b => b.UserId == userId)
                .Include(b => b.BookCopy)
                .ToList();
        }

        public void CheckOverdue()
        {
            var overdueBorrows = _context.Borrows
                .Where(b => b.ReturnDate == null && DateTime.Now > b.DueDate)
                .ToList();

            foreach (var b in overdueBorrows)
            {
                b.IsOverdue = true;
                b.OverdueDays = (int)(DateTime.Now - b.DueDate).TotalDays;
            }

            var nonOverdueBorrows = _context.Borrows
                .Where(b => b.ReturnDate != null || DateTime.Now <= b.DueDate)
                .ToList();

            foreach (var b in nonOverdueBorrows)
            {
                b.IsOverdue = false;
                b.OverdueDays = 0;
            }

            _context.SaveChanges();
        }

        public List<Borrow> Search(BorrowSearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;

            return _context.Borrows
                .Where(b =>
                    (dto.Number == null || b.Id == dto.Number) &&
                    (dto.UserId == null || b.UserId == dto.UserId) &&
                    (dto.BookCopyId == null || b.BookCopyId == dto.BookCopyId) &&
                    (dto.Returned == null ||
                        (dto.Returned.Value
                            ? b.ReturnDate != null
                            : b.ReturnDate == null))
                )
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
