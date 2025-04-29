using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    public static class BookBorrowingRequestMapper
    {
        public static BorrowingRequestDTO ToBookBorrowingRequestDTO(this BookBorrowingRequest bookBorrowingRequest)
        {
            return new BorrowingRequestDTO
            {
                Id = bookBorrowingRequest.Id,
                RequestorId = bookBorrowingRequest.RequestorId,
                ApproverId = bookBorrowingRequest.ApproverId,
                RequestedDate = bookBorrowingRequest.RequestedDate,
                Status = bookBorrowingRequest.Status
            };
        }

        public static BookBorrowingRequest ToBookBorrowingRequest(this BorrowingRequestDTO borrowingRequestDTO)
        {
            return new BookBorrowingRequest
            {
                Id = borrowingRequestDTO.Id,
                RequestorId = borrowingRequestDTO.RequestorId,
                ApproverId = borrowingRequestDTO.ApproverId,
                RequestedDate = borrowingRequestDTO.RequestedDate,
                Status = borrowingRequestDTO.Status
            };
        }
    }
}
