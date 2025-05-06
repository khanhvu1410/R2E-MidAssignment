using LibraryManagement.API.Controllers;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.API.Tests.Controllers
{
    [TestFixture]
    internal class BooksControllerTests
    {
        private Mock<IBookService> _mockBookService;
        private BooksController _booksController;

        [SetUp]
        public void Setup()
        {
            _mockBookService = new Mock<IBookService>();
            _booksController = new BooksController(_mockBookService.Object);
        }

        #region CreateBook Tests

        [Test]
        public async Task CreateBook_WithValidInput_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var bookToAdd = new BookToAddDTO { Title = "New Book" };
            var expectedBook = new BookToReturnDTO { Id = 1, Title = "New Book" };

            _mockBookService.Setup(x => x.AddBookAsync(bookToAdd))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _booksController.CreateBook(bookToAdd);

            // Assert          
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
                var createdAtResult = result.Result as CreatedAtActionResult;
                Assert.That(createdAtResult?.ActionName, Is.EqualTo(nameof(_booksController.GetBookById)));
                Assert.That(createdAtResult?.RouteValues?["id"], Is.EqualTo(expectedBook.Id));
                Assert.That(createdAtResult?.Value, Is.EqualTo(expectedBook));
            });
        }

        [Test]
        public void CreateBook_WhenServiceThrowsException_ReturnsProblem()
        {
            // Arrange
            var bookToAdd = new BookToAddDTO { Title = "New Book" };
            _mockBookService.Setup(x => x.AddBookAsync(bookToAdd))
                .ThrowsAsync(new Exception("Test exception"));
        
            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _booksController.CreateBook(bookToAdd));
            Assert.That(ex.Message, Is.EqualTo("Test exception"));
        }

        [Test]
        public void CreateBook_WithoutSuperUserRole_ReturnUnauthorized()
        {
            // Arrange
            var bookToAdd = new BookToAddDTO { Title = "New Book" };
            _mockBookService.Setup(x => x.AddBookAsync(bookToAdd))
            .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedException>(() => _booksController.CreateBook(bookToAdd));
            Assert.That(ex?.Message, Is.EqualTo("Unauthorized request"));
        }

        #endregion

        #region GetBooks Tests

        [Test]
        public async Task GetBooks_WithValidPagination_ReturnsPagedResponse()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var expectedResponse = new PagedResponse<BookToReturnDTO>
            {
                Items = new List<BookToReturnDTO>
                {
                    new BookToReturnDTO { Id = 1, Title = "Book 1" }
                },
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            _mockBookService.Setup(x => x.GetBooksPaginatedAsync(pageIndex, pageSize))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _booksController.GetBooks(pageIndex, pageSize);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public void GetBooks_WhenServiceThrowsException_ReturnsProblem()
        {
            // Arrange
            _mockBookService.Setup(x => x.GetBooksPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _booksController.GetBooks(1, 10));
            Assert.That(ex.Message, Is.EqualTo("Test exception"));
        }

        #endregion

        #region GetBookById Tests

        [Test]
        public async Task GetBookById_WithExistingId_ReturnsBook()
        {
            // Arrange
            var bookId = 1;
            var expectedBook = new BookToReturnDTO { Id = bookId, Title = "Existing Book" };

            _mockBookService.Setup(x => x.GetBookByIdAsync(bookId))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _booksController.GetBookById(bookId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(expectedBook));
            });
        }

        [Test]
        public void GetBookById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var bookId = 999;
            _mockBookService.Setup(x => x.GetBookByIdAsync(bookId))
                .ThrowsAsync(new NotFoundException("Book not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _booksController.GetBookById(bookId));
            Assert.That(ex.Message, Is.EqualTo("Book not found"));
        }

        #endregion

        #region UpdateBook Tests

        [Test]
        public async Task UpdateBook_WithValidInput_ReturnsUpdatedBook()
        {
            // Arrange
            var bookId = 1;
            var bookToUpdate = new BookToUpdateDTO { Title = "Updated Title" };
            var expectedBook = new BookToReturnDTO { Id = bookId, Title = "Updated Title" };

            _mockBookService.Setup(x => x.UpdateBookAsync(bookId, bookToUpdate))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _booksController.UpdateBook(bookId, bookToUpdate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(expectedBook));
            });
        }

        [Test]
        public void UpdateBook_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var bookId = 999;
            var bookToUpdate = new BookToUpdateDTO { Title = "Updated Title" };
            _mockBookService.Setup(x => x.UpdateBookAsync(bookId, bookToUpdate))
                .ThrowsAsync(new NotFoundException("Book not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _booksController.UpdateBook(bookId, bookToUpdate));
            Assert.That(ex.Message, Is.EqualTo("Book not found"));
        }

        [Test]
        public void UpdateBook_WithoutSuperUserRole_ReturnUnauthorized()
        {
            // Arrange
            var bookToUpdate = new BookToUpdateDTO { Title = "Updated Title" };
            _mockBookService.Setup(x => x.UpdateBookAsync(1, bookToUpdate))
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedException>(() => _booksController.UpdateBook(1, bookToUpdate));
            Assert.That(ex?.Message, Is.EqualTo("Unauthorized request"));
        }

        #endregion

        #region DeleteBook Tests

        [Test]
        public async Task DeleteBook_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var bookId = 1;

            // Act
            var result = await _booksController.DeleteBook(bookId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mockBookService.Verify(x => x.DeleteBookAsync(bookId), Times.Once);
        }

        [Test]
        public void DeleteBook_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var bookId = 999;
            _mockBookService.Setup(x => x.DeleteBookAsync(bookId))
                .ThrowsAsync(new NotFoundException("Book not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _booksController.DeleteBook(bookId));
            Assert.That(ex.Message, Is.EqualTo("Book not found"));
        }

        [Test]
        public void DeleteBook_WhenBookInUse_ReturnsBadRequest()
        {
            // Arrange
            var bookId = 1;
            _mockBookService.Setup(x => x.DeleteBookAsync(bookId))
                .ThrowsAsync(new BadRequestException("Book cannot be deleted"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() => _booksController.DeleteBook(bookId));
            Assert.That(ex.Message, Is.EqualTo("Book cannot be deleted"));
        }

        [Test]
        public void DeleteBook_WithoutSuperUserRole_ReturnUnauthorized()
        {
            _mockBookService.Setup(x => x.DeleteBookAsync(1))
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedException>(() => _booksController.DeleteBook(1));
            Assert.That(ex?.Message, Is.EqualTo("Unauthorized request"));
        }

        #endregion
    }
}
