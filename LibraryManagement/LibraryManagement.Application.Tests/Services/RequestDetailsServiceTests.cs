using System.Linq.Expressions;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Moq;

namespace LibraryManagement.Application.Tests.Services
{
    [TestFixture]
    internal class RequestDetailsServiceTests
    {
        private Mock<IGenericRepository<BookBorrowingRequestDetails>> _mockRequestDetailsRepository;
        private RequestDetailsService _requestDetailsService;

        [SetUp]
        public void Setup()
        {
            _mockRequestDetailsRepository = new Mock<IGenericRepository<BookBorrowingRequestDetails>>();
            _requestDetailsService = new RequestDetailsService(_mockRequestDetailsRepository.Object);
        }

        [Test]
        public async Task GetRequestDetailsByBorrowingRequestId_WithValidRequestId_ReturnsRequestDetails()
        {
            // Arrange
            var borrowingRequestId = 1;
            var requestDetails = new List<BookBorrowingRequestDetails>
            {
                new BookBorrowingRequestDetails
                {
                    BookBorrowingRequestId = borrowingRequestId,
                    Book = new Book { Id = 1, Title = "Book 1" }
                },
                new BookBorrowingRequestDetails
                {
                    BookBorrowingRequestId = borrowingRequestId,
                    Book = new Book { Id = 2, Title = "Book 2" }
                },
                new BookBorrowingRequestDetails
                {
                    BookBorrowingRequestId = 2, // Different request
                    Book = new Book { Id = 3, Title = "Book 3" }
                }
            };

            _mockRequestDetailsRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<BookBorrowingRequestDetails, object>>>()))
                .ReturnsAsync(requestDetails);

            // Act
            var result = await _requestDetailsService.GetRequestDetailsByBorrowingRequestId(borrowingRequestId);

            // Assert         
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(2));
                Assert.That(result.Any(rd => rd.Book?.Title == "Book 1"), Is.True);
                Assert.That(result.Any(rd => rd.Book?.Title == "Book 2"), Is.True);
                Assert.That(result.Any(rd => rd.Book?.Title == "Book 3"), Is.False);
            });
        }

        [Test]
        public async Task GetRequestDetailsByBorrowingRequestId_WithNoMatchingRequestId_ReturnsEmptyList()
        {
            // Arrange
            var borrowingRequestId = 99; // Non-existent request ID
            var requestDetails = new List<BookBorrowingRequestDetails>
            {
                new BookBorrowingRequestDetails
                {
                    BookBorrowingRequestId = 1,
                    Book = new Book { Id = 1, Title = "Book 1" }
                }
            };

            _mockRequestDetailsRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<BookBorrowingRequestDetails, object>>>()))
                .ReturnsAsync(requestDetails);

            // Act
            var result = await _requestDetailsService.GetRequestDetailsByBorrowingRequestId(borrowingRequestId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetRequestDetailsByBorrowingRequestId_WithNullBook_ReturnsDTOWithNullBook()
        {
            // Arrange
            var borrowingRequestId = 1;
            var requestDetails = new List<BookBorrowingRequestDetails>
            {
                new BookBorrowingRequestDetails
                {
                    BookBorrowingRequestId = borrowingRequestId,
                }
            };

            _mockRequestDetailsRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<BookBorrowingRequestDetails, object>>>()))
                .ReturnsAsync(requestDetails);

            // Act
            var result = await _requestDetailsService.GetRequestDetailsByBorrowingRequestId(borrowingRequestId);

            // Assert          
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.That(result.First().Book, Is.Null);
            });
        }

        [Test]
        public async Task GetRequestDetailsByBorrowingRequestId_WithEmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            var borrowingRequestId = 1;
            _mockRequestDetailsRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<BookBorrowingRequestDetails, object>>>()))
                .ReturnsAsync(new List<BookBorrowingRequestDetails>());

            // Act
            var result = await _requestDetailsService.GetRequestDetailsByBorrowingRequestId(borrowingRequestId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }
    }
}
