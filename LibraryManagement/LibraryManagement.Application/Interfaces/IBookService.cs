using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookService
    {
        public Task<BookDTO> AddBookAsync(BookToAddDTO bookToAddDTO);

        public Task<PagedResponse<BookDTO>> GetBooksPaginatedAsync(int pageIndex, int pageSize);

        public Task<BookDTO> GetBookByIdAsync(int id);   

        public Task<BookDTO> UpdateBookAsync(BookDTO bookDTO);

        public Task DeleteBookAsync(int id);
    }
}
