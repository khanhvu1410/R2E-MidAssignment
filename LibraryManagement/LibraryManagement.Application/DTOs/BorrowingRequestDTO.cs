using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs
{
    public class BorrowingRequestDTO
    {
        public int Id { get; set; }

        public int RequestorId { get; set; }

        public int? ApproverId { get; set; }

        public DateTime RequestedDate { get; set; } 

        public RequestStatus Status { get; set; }
    }
}
