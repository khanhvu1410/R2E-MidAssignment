using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities
{
    public class BookBorrowingRequest
    {
        public int Id { get; set; }

        public int RequestorId { get; set; }

        public int? ApproverId { get; set; }

        public DateTime RequestedDate { get; set; } 

        public RequestStatus Status { get; set; } = RequestStatus.Waiting;

        public User Requestor { get; set; } = default!;

        public User? Approver { get; set; }

        public ICollection<BookBorrowingRequestDetails>? BookBorrowingRequestDetails { get; set; }
    }
}
