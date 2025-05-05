namespace LibraryManagement.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = default!;

        public string Author { get; set; } = default!;

        public string ISBN { get; set; } = default!;

        public int PublicationYear { get; set; }

        public string? Description { get; set; } 

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = default!;

        public ICollection<BookBorrowingRequestDetails>? BookBorrowingRequestDetails { get; set; }
    }
}
