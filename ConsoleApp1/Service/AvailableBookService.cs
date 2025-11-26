using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AvailableBookDto;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class AvailableBookService
    {
        private List<AvailableBook> _inventory;
        private int _idCounter = 1;

        public AvailableBookService(List<AvailableBook> inventory)
        {
            _inventory = inventory;
        }

        public void AddAvailableBook(AvailableBookCreateDto dto)
        {
            AvailableBook a = new AvailableBook();
            a.Id = _idCounter++;
            a.BookId = dto.BookId;
            a.CreatedBy = 1;
            a.CreatedDate = DateTime.Now;
            _inventory.Add(a);
        }

        public List<AvailableBookListDto> ListAvailableBooks()
        {
            List<AvailableBookListDto> result = new List<AvailableBookListDto>();

            foreach (var a in _inventory)
            {
                AvailableBookListDto dto = new AvailableBookListDto();
                dto.Id = a.Id;
                dto.BookId = a.BookId;
                dto.IsAvailable = a.IsAvailable;

                result.Add(dto);
            }

            return result;
        }
            

            public int GetAvailableCount(int bookId)
            {
                return _inventory.Count(x => x.BookId == bookId && x.IsAvailable);
            }

            public int GetBorrowedCount(int bookId)
            {
                return _inventory.Count(x => x.BookId == bookId && !x.IsAvailable);
            }

            public List<AvailableBook> GetAllCopiesForBook(int bookId)
            {
                return _inventory.Where(x => x.BookId == bookId).ToList();
            }

            public AvailableBook GetSpecificCopy(int availableBookId)
            {
                return _inventory.FirstOrDefault(x => x.Id == availableBookId);
            }
        }
}
