namespace LibraryManagement.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Author { get; set; }

        public string? ISBN { get; set; }

        public int PublicationYear { get; set; }

        public string? Description { get; set; } 

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public ICollection<BookBorrowingRequestDetails>? BookBorrowingRequestDetails { get; set; }
    }
}
