using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    internal static class BookBorrowingRequestDetailsMapper
    {
        public static RequestDetailsDTO ToBookBorrowingRequestDetailsDTO(this BookBorrowingRequestDetails bookBorrowingRequestDetails)
        {
            return new RequestDetailsDTO
            {
                BookId = bookBorrowingRequestDetails.BookId,
            };
        }
    }
}
