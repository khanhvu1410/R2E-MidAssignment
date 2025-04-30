using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.BorrowingRequest
{
    public class BorrowingRequestToUpdateDTO
    {
        public int Id { get; set; }

        public RequestStatus Status { get; set; }
    }
}
