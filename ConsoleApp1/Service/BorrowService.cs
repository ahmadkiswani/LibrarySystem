using LibrarySystem.Models;
using LibrarySystem.DTOs.BorrowDTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class BorrowService
    {
        private readonly LibraryContext _context;
        private List<Borrow> _borrow => _context.Borrows;
        private List<BookCopy> _inventory => _context.BookCopies;
        private List<User> _user => _context.Users;
        private int _idCounter = 1;

        public BorrowService(LibraryContext context)
        {
            _context = context;
        }
        public void BorrowBook(BorrowCreateDto dto)
        {
            var copy = _inventory.FirstOrDefault(x => x.Id == dto.BookCopyId);

            if (copy == null || !copy.IsAvailable)
                throw new Exception("This copy is not available");
            if (!_user.Any(u => u.Id == dto.Id))
                throw new Exception("User does not exist");
            copy.LastModifiedBy = 1;
            copy.IsAvailable = false;
            copy.LastModifiedDate = DateTime.Now;

            Borrow b = new Borrow();
            b.Id = _idCounter++;
            b.UserId = dto.Id;
            b.BookCopyId = dto.BookCopyId;
            b.BorrowDate = DateTime.Now;
            b.DueDate = DateTime.Now.AddDays(5);  

            b.CreatedBy = 1;
            b.CreatedDate = DateTime.Now;
            _borrow.Add(b);
            var user = _user.FirstOrDefault(u => u.Id == dto.Id);

            if (user != null)
            {
                if (user.Borrows == null)
                    user.Borrows = new List<Borrow>();

                user.Borrows.Add(b);
            }
            if (copy.BorrowRecords == null)
                copy.BorrowRecords = new List<Borrow>();

            copy.BorrowRecords.Add(b);


        }

        public void ReturnBook( BorrowReturnDto dto)
        {
            var b = _borrow.FirstOrDefault(z => z.Id == dto.Id);

            if (b == null)
                throw new Exception("Borrow record not found");

            if (b.ReturnDate != null)
                throw new Exception("This book is already returned");

            b.ReturnDate = DateTime.Now;
            b.LastModifiedBy = 1;
            b.LastModifiedDate = DateTime.Now;

            var copy = _inventory.FirstOrDefault(x => x.Id == b.BookCopyId);

            if (copy != null)
            {
                copy.IsAvailable = true;
                copy.LastModifiedBy = 1;
                copy.LastModifiedDate = DateTime.Now;
            }
        }
        public List<Borrow> GetBorrowedBooksByUser(int userId)
        {
            return _borrow.Where(b => b.UserId == userId).ToList();
        }

        public void CheckOverdue()
        {
            _borrow
                .Where(b => b.ReturnDate == null && DateTime.Now > b.DueDate)
                .ToList()
                .ForEach(b =>
                {
                    b.IsOverdue = true;
                    b.OverdueDays = (int)(DateTime.Now - b.DueDate).TotalDays;
                });

            _borrow
                .Where(b => b.ReturnDate != null || DateTime.Now <= b.DueDate)
                .ToList()
                .ForEach(b =>
                {
                    b.IsOverdue = false;
                    b.OverdueDays = 0;
                });
        }
        public List<Borrow> Search(BorrowSearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;
            return _borrow
                .Where(b =>
                    (dto.Number == null || b.Id == dto.Number) &&
                    (dto.UserId == null || b.UserId == dto.UserId) &&
                    (dto.BookCopyId == null || b.BookCopyId == dto.BookCopyId) &&
                    (dto.Returned == null ||
                        (dto.Returned.Value
                            ? b.ReturnDate != null
                            : b.ReturnDate == null
                        )
                    )
                )
                .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                .ToList();
        }


    }

}

