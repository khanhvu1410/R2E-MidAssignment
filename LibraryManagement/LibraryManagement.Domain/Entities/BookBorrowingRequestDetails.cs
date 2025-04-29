namespace LibraryManagement.Domain.Entities
{
    public class BookBorrowingRequestDetails
    {
        public int BookBorrowingRequestId { get; set; }

        public int BookId { get; set; }
        
        public BookBorrowingRequest? BookBorrowingRequest { get; set; }

        public Book? Book { get; set; }
    }
}
