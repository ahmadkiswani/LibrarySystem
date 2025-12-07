using LibrarySystem.Shared.DTOs.BorrowDTOs;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.Services.Interfaces
{
    public interface IBorrowService
    {
        Task BorrowBook(BorrowCreateDto dto);
        Task ReturnBook(BorrowReturnDto dto);
        Task<List<Borrow>> Search(BorrowSearchDto dto);
    }
}
