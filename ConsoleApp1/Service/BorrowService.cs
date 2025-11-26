using LibrarySystem.Models;
using LibrarySystem.DTOs.BorrowDTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class BorrowService
    {
        private List<Borrow> _borrow;
        private List<AvailableBook> _inventory;
        private List<User>_user;
        private int _idCounter = 1;

        public BorrowService(List<Borrow> borrow, List<AvailableBook> inventory,List<User>user)
        {
            _borrow = borrow;
            _inventory = inventory;
            _user = user;
        }
        public void BorrowBook(BorrowCreateDto dto)
        {
            var copy = _inventory.FirstOrDefault(x => x.Id == dto.AvailableBookId);

            if (copy == null || !copy.IsAvailable)
                throw new Exception("This copy is not available");
            if (!_user.Any(u => u.Id == dto.UserId))
                throw new Exception("User does not exist");
            copy.LastModifiedBy = 1;
            copy.IsAvailable = false;
            copy.LastModifiedDate = DateTime.Now;

            Borrow b = new Borrow();
            b.Id = _idCounter++;
            b.UserId = dto.UserId;
            b.AvailableBookId = dto.AvailableBookId;
            b.BorrowDate = dto.BorrowDate;
            b.DueDate = dto.DueDate;
            b.CreatedBy = 1;
            b.CreatedDate = DateTime.Now;
            _borrow.Add(b);
        }

        public void ReturnBook(int borrowId, BorrowReturnDto dto)
        {
            Borrow b = _borrow.FirstOrDefault(z => z.Id == borrowId);

            if (b == null)
                throw new Exception("Borrow record not found");

            if (b.ReturnDate != null)
                throw new Exception("This book is already returned");

            b.ReturnDate = dto.ReturnDate;
            b.LastModifiedBy = 1;
            b.LastModifiedDate = DateTime.Now;

            var copy = _inventory.FirstOrDefault(x => x.Id == b.AvailableBookId);

            if (copy != null)
            {
                copy.IsAvailable = true;
                copy.LastModifiedBy = 1;
                copy.LastModifiedDate = DateTime.Now;
            }
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

    }

}

