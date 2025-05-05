using System.Security.Claims;
using LibraryManagement.API.Controllers;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.RequestDetails;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.API.Tests.Controllers
{
    [TestFixture]
    internal class RequestDetailsControllerTests
    {
        private Mock<IRequestDetailsService> _mockRequestDetailsService;
        private RequestDetailsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRequestDetailsService = new Mock<IRequestDetailsService>();
            _controller = new RequestDetailsController(_mockRequestDetailsService.Object);

            // Setup authorized user context
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "NormalUser")
            }, "TestAuthentication"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        #region GetRequestDetailsByBorrowingRequestId Tests

        [Test]
        public async Task GetRequestDetailsByBorrowingRequestId_WithValidRequestId_ReturnsRequestDetails()
        {
            // Arrange
            var requestId = 1;
            var expectedDetails = new List<RequestDetailsToReturnDTO>
            {
                new RequestDetailsToReturnDTO { Book = new BookToReturnDTO { Id = 1, Title = "Book 1" } },
                new RequestDetailsToReturnDTO { Book = new BookToReturnDTO { Id = 2, Title = "Book 2" } }
            };

            _mockRequestDetailsService.Setup(x =>
                x.GetRequestDetailsByBorrowingRequestId(requestId))
                .ReturnsAsync(expectedDetails);

            // Act
            var result = await _controller.GetRequestDetailsByBorrowingRequestId(requestId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(expectedDetails));
                _mockRequestDetailsService.Verify(x =>
                    x.GetRequestDetailsByBorrowingRequestId(requestId), Times.Once);
            });
        }

        [Test]
        public async Task GetRequestDetailsByBorrowingRequestId_WithNoDetails_ReturnsEmptyList()
        {
            // Arrange
            var requestId = 1;
            var emptyList = new List<RequestDetailsToReturnDTO>();

            _mockRequestDetailsService.Setup(x =>
                x.GetRequestDetailsByBorrowingRequestId(requestId))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetRequestDetailsByBorrowingRequestId(requestId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value as IEnumerable<RequestDetailsToReturnDTO>, Is.Empty);
            });
        }

        [Test]
        public void GetRequestDetailsByBorrowingRequestId_WhenServiceThrowsException_ReturnsProblem()
        {
            // Arrange
            var requestId = 1;
            _mockRequestDetailsService.Setup(x =>
                x.GetRequestDetailsByBorrowingRequestId(requestId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _controller.GetRequestDetailsByBorrowingRequestId(requestId));
            Assert.That(ex.Message, Is.EqualTo("Test exception"));
        }

        #endregion
    }
}
