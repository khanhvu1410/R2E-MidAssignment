namespace LibraryManagement.Application.DTOs.Book
{
    public class BookToUpdateDTO
    {
        public string? Title { get; set; }

        public string? Author { get; set; }

        public string? ISBN { get; set; }

        public int PublicationYear { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }
    }
}
