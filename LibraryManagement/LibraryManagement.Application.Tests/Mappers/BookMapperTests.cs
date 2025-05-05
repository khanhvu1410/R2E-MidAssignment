using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Tests.Mappers
{
    [TestFixture]
    internal class BookMapperTests
    {
        #region ToBookToReturnDTO Tests

        [Test]
        public void ToBookToReturnDTO_WithCompleteBook_ReturnsCorrectDTO()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                PublicationYear = 2023,
                Description = "Test Description",
                Quantity = 5,
                CategoryId = 1,
                Category = new Category { Id = 1, Name = "Fiction" }
            };

            // Act
            var result = book.ToBookToReturnDTO();

            // Assert            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(book.Id));
                Assert.That(result.Title, Is.EqualTo(book.Title));
                Assert.That(result.Author, Is.EqualTo(book.Author));
                Assert.That(result.ISBN, Is.EqualTo(book.ISBN));
                Assert.That(result.PublicationYear, Is.EqualTo(book.PublicationYear));
                Assert.That(result.Description, Is.EqualTo(book.Description));
                Assert.That(result.Quantity, Is.EqualTo(book.Quantity));
                Assert.That(result.CategoryId, Is.EqualTo(book.CategoryId));
                Assert.That(result.CategoryName, Is.EqualTo(book.Category.Name));
            });
        }

        [Test]
        public void ToBookToReturnDTO_WithNullDescription_ReturnsDTOWithNullDescription()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                PublicationYear = 2023,
                Description = null,
                Quantity = 5,
                CategoryId = 1,
                Category = new Category { Id = 1, Name = "Fiction" }
            };

            // Act
            var result = book.ToBookToReturnDTO();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(book.Id));
                Assert.That(result.Title, Is.EqualTo(book.Title));
                Assert.That(result.Author, Is.EqualTo(book.Author));
                Assert.That(result.ISBN, Is.EqualTo(book.ISBN));
                Assert.That(result.PublicationYear, Is.EqualTo(book.PublicationYear));
                Assert.That(result.Description, Is.Null);
                Assert.That(result.Quantity, Is.EqualTo(book.Quantity));
                Assert.That(result.CategoryId, Is.EqualTo(book.CategoryId));
                Assert.That(result.CategoryName, Is.EqualTo(book.Category.Name));
            });
        }

        #endregion

        #region ToBook Tests

        [Test]
        public void ToBook_WithCompleteBookToAddDTO_ReturnsCorrectBook()
        {
            // Arrange
            var bookToAddDTO = new BookToAddDTO
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                PublicationYear = 2023,
                Description = "Test Description",
                Quantity = 5,
                CategoryId = 1
            };

            // Act
            var result = bookToAddDTO.ToBook();

            // Assert           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Title, Is.EqualTo(bookToAddDTO.Title));
                Assert.That(result.Author, Is.EqualTo(bookToAddDTO.Author));
                Assert.That(result.ISBN, Is.EqualTo(bookToAddDTO.ISBN));
                Assert.That(result.PublicationYear, Is.EqualTo(bookToAddDTO.PublicationYear));
                Assert.That(result.Description, Is.EqualTo(bookToAddDTO.Description));
                Assert.That(result.Quantity, Is.EqualTo(bookToAddDTO.Quantity));
                Assert.That(result.CategoryId, Is.EqualTo(bookToAddDTO.CategoryId));
            });
        }

        [Test]
        public void ToBook_WithMinimumBookToAddDTO_ReturnsCorrectBook()
        {
            // Arrange
            var bookToAddDTO = new BookToAddDTO
            {
                Title = "Minimal Book"
            };

            // Act
            var result = bookToAddDTO.ToBook();

            // Assert          
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Title, Is.EqualTo(bookToAddDTO.Title));
                Assert.That(result.Author, Is.Null);
                Assert.That(result.ISBN, Is.Null);
                Assert.That(result.PublicationYear, Is.EqualTo(0));
                Assert.That(result.Description, Is.Null);
                Assert.That(result.Quantity, Is.EqualTo(0));
                Assert.That(result.CategoryId, Is.EqualTo(0));
            });
        }

        [Test]
        public void ToBook_WithNullBookToAddDTO_ReturnsNullBookOutput()
        {
            // Arrange
            BookToAddDTO? bookToAddDTO = null;

            // Act & Assert
            Assert.That(bookToAddDTO?.ToBook(), Is.Null);
        }

        #endregion
    }
}
