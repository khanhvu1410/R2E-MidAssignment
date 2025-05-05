using LibraryManagement.Application.DTOs.BorrowingRequest;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    public static class BorrowingRequestMapper
    {
        public static BorrowingRequestToReturnDTO ToBookBorrowingRequestToReturnDTO(this BookBorrowingRequest bookBorrowingRequest)
        {
            return new BorrowingRequestToReturnDTO
            {
                Id = bookBorrowingRequest.Id,
                RequestorId = bookBorrowingRequest.RequestorId,
                ApproverId = bookBorrowingRequest.ApproverId,
                RequestorUsername = bookBorrowingRequest.Requestor?.Username,
                ApproverUsername = bookBorrowingRequest.Approver?.Username ?? default!,
                RequestedDate = bookBorrowingRequest.RequestedDate,
                Status = bookBorrowingRequest.Status
            };
        }
    }
}
