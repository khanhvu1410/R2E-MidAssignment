using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Services
{
    public class BookBorrowingRequestDetailsService : IBookBorrowingRequestDetailsService
    {
        IGenericRepository<BookBorrowingRequestDetails> _requestDetailsRepository;

        public BookBorrowingRequestDetailsService(IGenericRepository<BookBorrowingRequestDetails> requestDetailsRepository)
        {
            _requestDetailsRepository = requestDetailsRepository;
        }

        public async Task<IEnumerable<RequestDetailsToReturnDTO>> GetRequestDetailsByBorrowingRequestId(int borrowingRequetsId)
        {
            var query = _requestDetailsRepository.GetQueryable();
            return await query
                .Where(rd => rd.BookBorrowingRequestId == borrowingRequetsId)
                .Include(rd => rd.Book)
                .Select(rd => new RequestDetailsToReturnDTO
                {
                    BookId = rd.BookId,
                    BookName = rd.Book != null ? rd.Book.Title : string.Empty,
                    BookQuantity = rd.Book != null ? rd.Book.Quantity : 0
                }).ToListAsync();
        }
    }
}
