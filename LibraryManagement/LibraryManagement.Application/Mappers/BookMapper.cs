using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    internal static class BookMapper
    {
        public static BookToReturnDTO ToBookToReturnDTO(this Book book)
        {
            return new BookToReturnDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                Description = book.Description,
                Quantity = book.Quantity,
                CategoryId = book.CategoryId,
                CategoryName = book.Category?.Name,
            };
        }

        public static Book ToBook(this BookToAddDTO bookToAddDTO)
        {
            return new Book
            {
                Title = bookToAddDTO.Title,
                Author = bookToAddDTO.Author,
                ISBN = bookToAddDTO.ISBN,
                PublicationYear = bookToAddDTO.PublicationYear,
                Description = bookToAddDTO.Description,
                Quantity = bookToAddDTO.Quantity,
                CategoryId = bookToAddDTO.CategoryId,
            };
        }
    }
}
