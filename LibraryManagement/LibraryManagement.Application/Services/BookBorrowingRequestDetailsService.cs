using LibraryManagement.Application.DTOs.RequestDetails;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class BookBorrowingRequestDetailsService : IBookBorrowingRequestDetailsService
    {
        IGenericRepository<BookBorrowingRequestDetails> _requestDetailsRepository;

        public BookBorrowingRequestDetailsService(IGenericRepository<BookBorrowingRequestDetails> requestDetailsRepository)
        {
            _requestDetailsRepository = requestDetailsRepository;
        }

        public async Task<IEnumerable<RequestDetailsToReturnDTO>> GetRequestDetailsByBorrowingRequestId(int borrowingRequestId)
        {
            var requestDetails = await _requestDetailsRepository.GetAllAsync(rd => rd.Book);
            return requestDetails
                .Where(rd => rd.BookBorrowingRequestId == borrowingRequestId)
                .Select(rd => new RequestDetailsToReturnDTO
                {
                    Book = rd.Book?.ToBookToReturnDTO()
                });
        }
    }
}
