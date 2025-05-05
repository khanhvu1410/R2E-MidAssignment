using System.Security.Claims;
using LibraryManagement.API.Controllers;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.BorrowingRequest;
using LibraryManagement.Application.DTOs.RequestDetails;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.API.Tests.Controllers
{
    [TestFixture]
    internal class BorrowingRequestsControllerTests
    {
        private Mock<IBorrowingRequestService> _mockBorrowingRequestService;
        private BorrowingRequestsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockBorrowingRequestService = new Mock<IBorrowingRequestService>();
            _controller = new BorrowingRequestsController(_mockBorrowingRequestService.Object);
        }

        private void SetupUserContext(string role, int userId = 1)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        #region CreateBorrowingRequest Tests

        [Test]
        public async Task CreateBorrowingRequest_WithValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            SetupUserContext("NormalUser");
            var requestDetails = new List<RequestDetailsToAddDTO> { new RequestDetailsToAddDTO { BookId = 1 } };
            var expectedResponse = new BorrowingRequestToReturnDTO { Id = 1, RequestorId = 1 };

            _mockBorrowingRequestService.Setup(x =>
                x.AddBorrowingRequestAsync(1, requestDetails))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateBorrowingRequest(requestDetails);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
                var createdAtResult = result.Result as CreatedAtActionResult;
                Assert.That(createdAtResult?.ActionName, Is.EqualTo(nameof(_controller.GetBookBorrowingRequestById)));
                Assert.That(createdAtResult?.RouteValues?["id"], Is.EqualTo(expectedResponse.Id));
                Assert.That(createdAtResult?.Value, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public void CreateBorrowingRequest_WithoutNormalUserRole_ReturnsUnauthorized()
        {
            // Arrange
            SetupUserContext("SuperUser");
            var requestDetails = new List<RequestDetailsToAddDTO>();

            _mockBorrowingRequestService.Setup(x => x.AddBorrowingRequestAsync(1, requestDetails))
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedException>(() => _controller.CreateBorrowingRequest(requestDetails));
            Assert.That(ex?.Message, Is.EqualTo("Unauthorized request"));
        }

        [Test]
        public void CreateBorrowingRequest_WhenServiceThrowsException_ReturnsProblem()
        {
            // Arrange
            SetupUserContext("NormalUser");
            var requestDetails = new List<RequestDetailsToAddDTO>();
            _mockBorrowingRequestService.Setup(x =>
                x.AddBorrowingRequestAsync(1, requestDetails))
                .ThrowsAsync(new Exception("Test error"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _controller.CreateBorrowingRequest(requestDetails));
            Assert.That(ex?.Message, Is.EqualTo("Test error"));
        }

        #endregion

        #region GetBorrowingRequests Tests

        [Test]
        public async Task GetBorrowingRequests_WithSuperUserRole_ReturnsPagedResponse()
        {
            // Arrange
            SetupUserContext("SuperUser");
            var expectedResponse = new PagedResponse<BorrowingRequestToReturnDTO>
            {
                Items = new List<BorrowingRequestToReturnDTO>(),
                PageIndex = 1,
                PageSize = 10
            };

            _mockBorrowingRequestService
                .Setup(x => x.GetBorrowingRequestsPaginatedAsync(1, 10))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetBorrowingRequests(1, 10);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That((result.Result as OkObjectResult)?.Value, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public void GetBorrowingRequests_WithoutSuperUserRole_ReturnsUnauthorized()
        {
            // Arrange
            SetupUserContext("NormalUser");
            _mockBorrowingRequestService
                .Setup(x => x.GetBorrowingRequestsPaginatedAsync(1, 10))
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedException>(() => _controller.GetBorrowingRequests(1, 10));
            Assert.That(ex?.Message, Is.EqualTo("Unauthorized request"));
        }

        #endregion

        #region GetBookBorrowingRequestById Tests

        [Test]
        public async Task GetBookBorrowingRequestById_WithSuperUserRole_ReturnsRequest()
        {
            // Arrange
            SetupUserContext("SuperUser");
            var expectedResponse = new BorrowingRequestToReturnDTO { Id = 1 };

            _mockBorrowingRequestService
                .Setup(x => x.GetBorrowingRequestByIdAsync(1))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetBookBorrowingRequestById(1);
            
            // Assert
            Assert.Multiple(() =>
            {                
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That((result.Result as OkObjectResult)?.Value, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public void GetBookBorrowingRequestById_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            SetupUserContext("SuperUser");
            _mockBorrowingRequestService.Setup(x => x.GetBorrowingRequestByIdAsync(1))
                .ThrowsAsync(new NotFoundException("Not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _controller.GetBookBorrowingRequestById(1));
            Assert.That(ex?.Message, Is.EqualTo("Not found"));
        }

        #endregion

        #region GetBorrowingRequestsThisMonth Tests

        [Test]
        public async Task GetBorrowingRequestsThisMonth_WithNormalUserRole_ReturnsRequests()
        {
            // Arrange
            SetupUserContext("NormalUser");
            var expectedResponse = new List<BorrowingRequestToReturnDTO>
            {
                new BorrowingRequestToReturnDTO { Id = 1 }
            };

            _mockBorrowingRequestService
                .Setup(x => x.GetBorrowingRequestsThisMonthAsync(1))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetBorrowingRequestsThisMonth();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That((result.Result as OkObjectResult)?.Value, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public void GetBorrowingRequestsThisMonth_WithoutNormalUserRole_ReturnsUnauthorized()
        {
            // Arrange
            SetupUserContext("SuperUser");
            _mockBorrowingRequestService
                .Setup(x => x.GetBorrowingRequestsThisMonthAsync(1))
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedException>(() => _controller.GetBorrowingRequestsThisMonth());
            Assert.That(ex?.Message, Is.EqualTo("Unauthorized request"));
        }

        #endregion

        #region GetBorrowingRequestsByRequestorId Tests

        [Test]
        public async Task GetBorrowingRequestsByRequestorId_WithNormalUserRole_ReturnsPagedResponse()
        {
            // Arrange
            SetupUserContext("NormalUser");
            var expectedResponse = new PagedResponse<BorrowingRequestToReturnDTO>
            {
                Items = new List<BorrowingRequestToReturnDTO>(),
                PageIndex = 1,
                PageSize = 10
            };

            _mockBorrowingRequestService.Setup(x =>
                x.GetBorrowingRequestsByRequestorId(1, 10, 1))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetBorrowingRequestsByRequestorId(1, 10);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That((result.Result as OkObjectResult)?.Value, Is.EqualTo(expectedResponse));
            });
        }

        #endregion

        #region UpdateBookBorrowingRequest Tests

        [Test]
        public async Task UpdateBookBorrowingRequest_WithSuperUserRole_ReturnsUpdatedRequest()
        {
            // Arrange
            SetupUserContext("SuperUser", 2);
            var updateDto = new BorrowingRequestToUpdateDTO { Status = RequestStatus.Approved };
            var expectedResponse = new BorrowingRequestToReturnDTO { Id = 1, ApproverId = 2 };

            _mockBorrowingRequestService
                .Setup(x => x.UpdateBorrowingRequestAsync(1, 2, updateDto))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.UpdateBookBorrowingRequest(1, updateDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public void UpdateBookBorrowingRequest_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            SetupUserContext("SuperUser");
            var updateDto = new BorrowingRequestToUpdateDTO();
            _mockBorrowingRequestService.Setup(x =>
                x.UpdateBorrowingRequestAsync(1, 1, updateDto))
                .ThrowsAsync(new NotFoundException("Not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _controller.UpdateBookBorrowingRequest(1, updateDto));
            Assert.That(ex?.Message, Is.EqualTo("Not found"));
        }

        #endregion
    }
}
