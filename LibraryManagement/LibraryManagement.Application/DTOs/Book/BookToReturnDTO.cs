using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.Book
{
    public class BookToReturnDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Author { get; set; }

        public string? ISBN { get; set; }

        public int PublicationYear { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
