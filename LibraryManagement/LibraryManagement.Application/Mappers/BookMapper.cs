using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    internal static class BookMapper
    {
        public static BookDTO ToBookDTO(this Book book)
        {
            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                Description = book.Description,
                Quantity = book.Quantity,
                CategoryId = book.CategoryId,
            };
        }

        public static Book ToBook(this BookDTO bookDTO)
        {
            return new Book
            {
                Id = bookDTO.Id,
                Title = bookDTO.Title,
                Author = bookDTO.Author,
                ISBN = bookDTO.ISBN,
                PublicationYear = bookDTO.PublicationYear,
                Description = bookDTO.Description,
                Quantity = bookDTO.Quantity,
                CategoryId = bookDTO.CategoryId,
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
