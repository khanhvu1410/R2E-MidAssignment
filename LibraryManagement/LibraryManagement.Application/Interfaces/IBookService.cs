using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Book;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookService
    {
        public Task<BookToReturnDTO> AddBookAsync(BookToAddDTO bookToAddDTO);

        public Task<PagedResponse<BookToReturnDTO>> GetBooksPaginatedAsync(int pageIndex, int pageSize);

        public Task<BookToReturnDTO> GetBookByIdAsync(int id);   

        public Task<BookToReturnDTO> UpdateBookAsync(int id, BookToUpdateDTO bookToUpdateDTO);

        public Task DeleteBookAsync(int id);
    }
}
