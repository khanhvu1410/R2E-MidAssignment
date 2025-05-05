using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.Book
{
    public class BookToReturnDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = default!;

        public string Author { get; set; } = default!;

        public string ISBN { get; set; } = default!;

        public int PublicationYear { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
