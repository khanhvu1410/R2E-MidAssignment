using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    internal static class BookBorrowingRequestDetailsMapper
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
