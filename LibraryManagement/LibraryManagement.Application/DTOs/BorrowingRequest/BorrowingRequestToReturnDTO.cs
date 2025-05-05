using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.BorrowingRequest
{
    public class BorrowingRequestToReturnDTO
    {
        public int Id { get; set; }

        public int RequestorId { get; set; }

        public int? ApproverId { get; set; }

        public string RequestorUsername { get; set; } = default!;

        public string ApproverUsername { get; set; } = default!;

        public DateTime RequestedDate { get; set; }

        public RequestStatus Status { get; set; }
    }
}
