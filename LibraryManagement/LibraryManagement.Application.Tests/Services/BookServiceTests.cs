using FluentValidation;
using FluentValidation.Results;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Services;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace LibraryManagement.Application.Tests.Services
{
    [TestFixture]
    internal class BookServiceTests
    {
        private Mock<IGenericRepository<Book>> _mockBookRepository;
        private Mock<IGenericRepository<BookBorrowingRequestDetails>> _mockRequestDetailsRepository;
        private Mock<IValidator<BookToAddDTO>> _mockBookToAddDTOValidator;
        private Mock<IValidator<BookToUpdateDTO>> _mockBookToUpdateDTOValidator;
        private BookService _bookService;

        [SetUp]
        public void Setup()
        {
            _mockBookRepository = new Mock<IGenericRepository<Book>>();
            _mockRequestDetailsRepository = new Mock<IGenericRepository<BookBorrowingRequestDetails>>();
            _mockBookToAddDTOValidator = new Mock<IValidator<BookToAddDTO>>();
            _mockBookToUpdateDTOValidator = new Mock<IValidator<BookToUpdateDTO>>();

            _bookService = new BookService(
                _mockBookRepository.Object,
                _mockRequestDetailsRepository.Object,
                _mockBookToAddDTOValidator.Object,
                _mockBookToUpdateDTOValidator.Object
            );
        }

        #region AddBookAsync Tests

        [Test]
        public async Task AddBookAsync_WithValidData_ReturnsBookToReturnDTO()
        {
            // Arrange
            var bookToAdd = new BookToAddDTO
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                PublicationYear = 2023,
                Description = "Test Description",
                Quantity = 5,
                CategoryId = 1
            };

            var validationResult = new ValidationResult();
            _mockBookToAddDTOValidator.Setup(v => v.ValidateAsync(bookToAdd, default))
                .ReturnsAsync(validationResult);

            var expectedBook = bookToAdd.ToBook();
            expectedBook.Id = 1;
            _mockBookRepository.Setup(r => r.AddAsync(It.IsAny<Book>()))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _bookService.AddBookAsync(bookToAdd);

            // Assert            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(expectedBook.Id));
                Assert.That(result.Title, Is.EqualTo(expectedBook.Title));
            });
            _mockBookToAddDTOValidator.Verify(v => v.ValidateAsync(bookToAdd, default), Times.Once);
            _mockBookRepository.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
        }

        [Test]
        public void AddBookAsync_WithInvalidData_ThrowsValidationException()
        {
            // Arrange
            var bookToAdd = new BookToAddDTO();
            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Title", "Title is required"),
                new ValidationFailure("Author", "Author is required")
            };
            var validationResult = new ValidationResult(validationErrors);

            _mockBookToAddDTOValidator.Setup(v => v.ValidateAsync(bookToAdd, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => _bookService.AddBookAsync(bookToAdd));
            Assert.That(ex.Errors.Count(), Is.EqualTo(validationErrors.Count));
            _mockBookRepository.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Never);
        }

        #endregion

        #region DeleteBookAsync Tests

        [Test]
        public async Task DeleteBookAsync_WithExistingBook_DeletesBook()
        {
            // Arrange
            var bookId = 1;
            var existingBook = new Book { Id = bookId };

            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync(existingBook);

            _mockRequestDetailsRepository.Setup(r => r.ExistsAsync(rd => rd.BookId == bookId))
                .ReturnsAsync(false);

            // Act
            await _bookService.DeleteBookAsync(bookId);

            // Assert
            _mockBookRepository.Verify(r => r.DeleteAsync(bookId), Times.Once);
        }

        [Test]
        public void DeleteBookAsync_WithNonExistingBook_ThrowsNotFoundException()
        {
            // Arrange
            var bookId = 999;
            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync(null as Book);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _bookService.DeleteBookAsync(bookId));
            Assert.That(ex.Message, Is.EqualTo($"Book with ID {bookId} not found."));
            _mockBookRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void DeleteBookAsync_WhenBookIsBorrowed_ThrowsBadRequestException()
        {
            // Arrange
            var bookId = 1;
            var existingBook = new Book { Id = bookId };

            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync(existingBook);

            _mockRequestDetailsRepository.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<BookBorrowingRequestDetails, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() => _bookService.DeleteBookAsync(bookId));
            Assert.That(ex.Message, Is.EqualTo($"Book with ID {bookId} cannot be deleted."));
            _mockBookRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        #endregion

        #region GetBooksPaginatedAsync Tests

        [Test]
        public async Task GetBooksPaginatedAsync_ReturnsPagedResponse()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var totalRecords = 2;

            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Category = new Category { Id = 1, Name = "Fiction" } },
                new Book { Id = 2, Title = "Book 2", Category = new Category { Id = 1, Name = "Fiction" } }
            };

            var pagedResult = new PagedResult<Book>(books, pageIndex, pageSize, totalRecords);
           
            _mockBookRepository.Setup(r => r.GetPagedAsync(pageIndex, pageSize, null, It.IsAny<Expression<Func<Book, object?>>>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _bookService.GetBooksPaginatedAsync(pageIndex, pageSize);

            // Assert
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.PageIndex, Is.EqualTo(pageIndex));
                Assert.That(result.PageSize, Is.EqualTo(pageSize));
                Assert.That(result.Items.Count(), Is.EqualTo(2));
                Assert.That(result.Items.ElementAt(0).Title, Is.EqualTo("Book 1"));
                Assert.That(result.Items.ElementAt(0).CategoryName, Is.EqualTo("Fiction"));
            });
        }

        #endregion

        #region GetBookByIdAsync Tests

        [Test]
        public async Task GetBookByIdAsync_WithExistingBook_ReturnsBookToReturnDTO()
        {
            // Arrange
            var bookId = 1;
            var book = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Category = new Category { Id = 1, Name = "Fiction" }
            };

            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId, It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(book);

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(bookId));
                Assert.That(result.Title, Is.EqualTo("Test Book"));
                Assert.That(result.CategoryName, Is.EqualTo("Fiction"));
            });
        }

        [Test]
        public void GetBookByIdAsync_WithNonExistingBook_ThrowsNotFoundException()
        {
            // Arrange
            var bookId = 999;
            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId, It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(null as Book);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _bookService.GetBookByIdAsync(bookId));
            Assert.That(ex.Message, Is.EqualTo($"Book with ID {bookId} not found."));
        }

        #endregion

        #region UpdateBookAsync Tests

        [Test]
        public async Task UpdateBookAsync_WithValidData_ReturnsUpdatedBook()
        {
            // Arrange
            var bookId = 1;
            var bookToUpdate = new BookToUpdateDTO
            {
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "9876543210",
                PublicationYear = 2023,
                Description = "Updated Description",
                Quantity = 3,
                CategoryId = 2,              
            };

            var validationResult = new ValidationResult();
            _mockBookToUpdateDTOValidator.Setup(v => v.ValidateAsync(bookToUpdate, default))
                .ReturnsAsync(validationResult);

            var existingBook = new Book { Id = bookId };
            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync(existingBook);

            _mockBookRepository.Setup(r => r.UpdateAsync(It.IsAny<Book>()))
                .ReturnsAsync((Book b) => b);

            // Act
            var result = await _bookService.UpdateBookAsync(bookId, bookToUpdate);

            // Assert          
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(bookId));
                Assert.That(result.Title, Is.EqualTo("Updated Title"));
                Assert.That(result.Author, Is.EqualTo("Updated Author"));
                Assert.That(result.PublicationYear, Is.EqualTo(2023));
                Assert.That(result.Description, Is.EqualTo("Updated Description"));
                Assert.That(result.Quantity, Is.EqualTo(3));
                Assert.That(result.CategoryId, Is.EqualTo(2));
            });
            _mockBookToUpdateDTOValidator.Verify(v => v.ValidateAsync(bookToUpdate, default), Times.Once);
            _mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Once);
        }

        [Test]
        public void UpdateBookAsync_WithInvalidData_ThrowsValidationException()
        {
            // Arrange
            var bookId = 1;
            var bookToUpdate = new BookToUpdateDTO();

            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Title", "Title is required")
            };
            var validationResult = new ValidationResult(validationErrors);

            _mockBookToUpdateDTOValidator.Setup(v => v.ValidateAsync(bookToUpdate, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => _bookService.UpdateBookAsync(bookId, bookToUpdate));
            Assert.That(ex.Errors.Count(), Is.EqualTo(1));
            _mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Test]
        public void UpdateBookAsync_WithNonExistingBook_ThrowsNotFoundException()
        {
            // Arrange
            var bookId = 999;
            var bookToUpdate = new BookToUpdateDTO();

            var validationResult = new ValidationResult();
            _mockBookToUpdateDTOValidator.Setup(v => v.ValidateAsync(bookToUpdate, default))
                .ReturnsAsync(validationResult);

            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync(null as Book);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _bookService.UpdateBookAsync(bookId, bookToUpdate));
            Assert.That(ex.Message, Is.EqualTo($"Book with ID {bookId} not found."));
            _mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        #endregion
    }
}
