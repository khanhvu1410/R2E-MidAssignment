using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Mappers;

namespace LibraryManagement.Application.Tests.Mappers
{
    [TestFixture]
    internal class RequestDetailsMapperTests
    {
        [Test]
        public void ToBookBorrowingRequestDetailsDTO_WithValidDetails_ReturnsCorrectDTO()
        {
            // Arrange
            var requestDetails = new BookBorrowingRequestDetails
            {
                BookId = 5,
                BookBorrowingRequestId = 10,
                Book = new Book { Id = 5, Title = "Sample Book" }
            };

            // Act
            var result = requestDetails.ToBookBorrowingRequestDetailsDTO();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.BookId, Is.EqualTo(requestDetails.BookId));
            });            
        }

        [Test]
        public void ToBookBorrowingRequestDetailsDTO_WithMinimumData_ReturnsCorrectDTO()
        {
            // Arrange
            var requestDetails = new BookBorrowingRequestDetails
            {
                BookId = 1
            };

            // Act
            var result = requestDetails.ToBookBorrowingRequestDetailsDTO();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.BookId, Is.EqualTo(1));
            });
        }

        [Test]
        public void ToBookBorrowingRequestDetailsDTO_WithNullInput_ReturnsNullOutput()
        {
            // Arrange
            BookBorrowingRequestDetails? requestDetails = null;

            // Act & Assert
            Assert.That(requestDetails?.ToBookBorrowingRequestDetailsDTO(), Is.Null);
        }

        [Test]
        public void ToBookBorrowingRequestDetailsDTO_DoesNotMapExtraProperties()
        {
            // Arrange
            var requestDetails = new BookBorrowingRequestDetails
            {
                BookId = 3,
                BookBorrowingRequestId = 7,
                Book = new Book { Id = 3, Title = "Extra Properties" }
            };

            // Act
            var result = requestDetails.ToBookBorrowingRequestDetailsDTO();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.BookId, Is.EqualTo(3));
                var dtoProperties = result.GetType().GetProperties();
                Assert.That(dtoProperties, Has.Length.EqualTo(1)); // Only BookId should be mapped
                Assert.That(dtoProperties[0].Name, Is.EqualTo("BookId"));
            });
        }

        [Test]
        public void ToBookBorrowingRequestDetailsDTO_WithDefaultValues_ReturnsCorrectDTO()
        {
            // Arrange
            var requestDetails = new BookBorrowingRequestDetails
            {
                BookId = 0 // default value
            };

            // Act
            var result = requestDetails.ToBookBorrowingRequestDetailsDTO();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.BookId, Is.EqualTo(0));
            });
        }
    }
}
