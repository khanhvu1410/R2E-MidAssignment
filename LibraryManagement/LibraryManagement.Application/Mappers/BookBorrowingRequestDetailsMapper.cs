using LibraryManagement.Application.DTOs.RequestDetails;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    public static class BookBorrowingRequestDetailsMapper
    {
        public static RequestDetailsToAddDTO ToBookBorrowingRequestDetailsDTO(this BookBorrowingRequestDetails bookBorrowingRequestDetails)
        {
            return new RequestDetailsToAddDTO
            {
                BookId = bookBorrowingRequestDetails.BookId,
            };
        }
    }
}
