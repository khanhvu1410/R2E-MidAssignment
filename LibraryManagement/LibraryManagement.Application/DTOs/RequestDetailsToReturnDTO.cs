namespace LibraryManagement.Application.DTOs
{
    public class RequestDetailsToReturnDTO
    {
        public int BookId { get; set; }

        public string? BookName { get; set; }

        public int BookQuantity { get; set; }
    }
}
