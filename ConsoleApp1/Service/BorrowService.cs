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

        public BorrowService(List<Borrow> borrow, List<AvailableBook> inventory)
        {
            _borrow = borrow;
            _inventory = inventory;
        }

        public void BorrowBook(BorrowCreateDto dto)
        {
            var copy = _inventory.FirstOrDefault(x => x.Id == dto.AvailableBookId);

            if (copy == null || !copy.IsAvailable)
                throw new Exception("This copy is not available");

            copy.IsAvailable = false;
            copy.LastModifiedBy = 1;
            copy.LastModifiedDate = DateTime.Now;

            Borrow b = new Borrow();
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

            if (b != null)
            {
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
        }
        public void CheckOverdue()
        {
            foreach (var b in _borrow)
            {
                if (b.ReturnDate == null && DateTime.Now > b.DueDate)
                {
                    b.IsOverdue = true;
                    b.OverdueDays = (int)(DateTime.Now - b.DueDate).TotalDays;
                }
                else
                {
                    b.IsOverdue = false;
                    b.OverdueDays = 0;
                }
            }
        }

    }
}
