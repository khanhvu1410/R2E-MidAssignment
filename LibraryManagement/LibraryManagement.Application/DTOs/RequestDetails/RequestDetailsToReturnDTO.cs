using LibraryManagement.Application.DTOs.Book;

namespace LibraryManagement.Application.DTOs.RequestDetails
{
    public class RequestDetailsToReturnDTO
    {
        public BookToReturnDTO? Book { get; set; }
    }
}
