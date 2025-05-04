using System.Linq.Expressions;
using LibraryManagement.Application.DTOs.BorrowingRequest;
using LibraryManagement.Application.DTOs.RequestDetails;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace LibraryManagement.Application.Tests.Services
{
    [TestFixture]
    internal class BookBorrowingRequestServiceTests
    {
        private Mock<IGenericRepository<BookBorrowingRequest>> _mockRequestRepository;
        private Mock<IGenericRepository<BookBorrowingRequestDetails>> _mockRequestDetailsRepository;
        private Mock<IGenericRepository<Book>> _mockBookRepository;
        private BookBorrowingRequestService _requestService;

        [SetUp]
        public void Setup()
        {
            _mockRequestRepository = new Mock<IGenericRepository<BookBorrowingRequest>>();
            _mockRequestDetailsRepository = new Mock<IGenericRepository<BookBorrowingRequestDetails>>();
            _mockBookRepository = new Mock<IGenericRepository<Book>>();

            _requestService = new BookBorrowingRequestService(
                _mockRequestRepository.Object,
                _mockRequestDetailsRepository.Object,
                _mockBookRepository.Object
            );
        }

        #region AddBookBorrowingRequestAsync Tests

        [Test]
        public async Task AddBookBorrowingRequestAsync_WithValidData_CreatesRequest()
        {
            // Arrange
            var requestorId = 1;
            var requestDetails = new List<RequestDetailsToAddDTO>
            {
                new RequestDetailsToAddDTO { BookId = 1 },
                new RequestDetailsToAddDTO { BookId = 2 }
            };

            var books = new List<Book>
            {
                new Book { Id = 1, Quantity = 5 },
                new Book { Id = 2, Quantity = 3 }
            };

            _mockRequestRepository.Setup(r => r.GetFiltersAsync(
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>()))
                .ReturnsAsync(new List<BookBorrowingRequest>());

            _mockRequestRepository.Setup(r => r.BeginTransactionAsync())
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());
            
            _mockBookRepository.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(books);

            _mockRequestRepository.Setup(r => r.AddAsync(It.IsAny<BookBorrowingRequest>()))
                .ReturnsAsync((BookBorrowingRequest r) => { r.Id = 1; return r; });

            // Act
            var result = await _requestService.AddBookBorrowingRequestAsync(requestorId, requestDetails);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RequestStatus.Waiting));
            _mockRequestRepository.Verify(r => r.AddAsync(It.IsAny<BookBorrowingRequest>()), Times.Once);
            _mockRequestDetailsRepository.Verify(r => r.AddAsync(It.IsAny<BookBorrowingRequestDetails>()), Times.Exactly(2));
            _mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Exactly(2));
        }

        [Test]
        public void AddBookBorrowingRequestAsync_ExceedsMonthlyLimit_ThrowsBadRequestException()
        {
            // Arrange
            var requestorId = 1;
            var requestDetails = new List<RequestDetailsToAddDTO> { new RequestDetailsToAddDTO { BookId = 1 } };

            var existingRequests = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest(),
                new BookBorrowingRequest(),
                new BookBorrowingRequest()
            };

            _mockRequestRepository.Setup(r => r.BeginTransactionAsync())
               .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            _mockRequestRepository.Setup(r => r.GetFiltersAsync(
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>()))
                .ReturnsAsync(existingRequests);           

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() =>
                _requestService.AddBookBorrowingRequestAsync(requestorId, requestDetails));
            Assert.That(ex.Message, Is.EqualTo("Maximum 3 borrowing requests per month."));
        }

        [Test]
        public void AddBookBorrowingRequestAsync_ExceedsBookLimit_ThrowsBadRequestException()
        {
            // Arrange
            var requestorId = 1;
            var requestDetails = Enumerable.Range(1, 6)
                .Select(i => new RequestDetailsToAddDTO { BookId = i })
                .ToList();

            _mockRequestRepository.Setup(r => r.BeginTransactionAsync())
               .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            _mockRequestRepository.Setup(r => r.GetFiltersAsync(
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>()))
                .ReturnsAsync(new List<BookBorrowingRequest>());

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() =>
                _requestService.AddBookBorrowingRequestAsync(requestorId, requestDetails));
            Assert.That(ex.Message, Is.EqualTo("Maximum 5 books can be requested at a time."));
        }

        [Test]
        public void AddBookBorrowingRequestAsync_BookNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var requestorId = 1;
            var requestDetails = new List<RequestDetailsToAddDTO> { new RequestDetailsToAddDTO { BookId = 99 } };

            _mockRequestRepository.Setup(r => r.BeginTransactionAsync())
               .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            _mockRequestRepository.Setup(r => r.GetFiltersAsync(
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>()))
                .ReturnsAsync(new List<BookBorrowingRequest>()); 

            _mockBookRepository.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new List<Book>());

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _requestService.AddBookBorrowingRequestAsync(requestorId, requestDetails));
            Assert.That(ex.Message, Is.EqualTo("Book with ID 99 not found."));
        }

        [Test]
        public void AddBookBorrowingRequestAsync_BookUnavailable_ThrowsBadRequestException()
        {
            // Arrange
            var requestorId = 1;
            var requestDetails = new List<RequestDetailsToAddDTO> { new RequestDetailsToAddDTO { BookId = 1 } };

            var books = new List<Book> { new Book { Id = 1, Quantity = 0 } };

            _mockRequestRepository.Setup(r => r.BeginTransactionAsync())
               .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            _mockRequestRepository.Setup(r => r.GetFiltersAsync(
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>()))
                .ReturnsAsync(new List<BookBorrowingRequest>());

            _mockBookRepository.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(books);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() =>
                _requestService.AddBookBorrowingRequestAsync(requestorId, requestDetails));
            Assert.That(ex.Message, Is.EqualTo("Book with ID 1 is unavailable."));
        }

        #endregion

        #region GetBookBorrowingRequestsPaginatedAsync Tests

        [Test]
        public async Task GetBookBorrowingRequestsPaginatedAsync_ReturnsPagedResponse()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var totalRecords = 2;
            var requests = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { Id = 1, Status = RequestStatus.Waiting },
                new BookBorrowingRequest { Id = 2, Status = RequestStatus.Approved }
            };

            var pagedResult = new PagedResult<BookBorrowingRequest>(requests, pageIndex, pageSize, totalRecords);

            _mockRequestRepository.Setup(r => r.GetPagedAsync(
                    pageIndex, pageSize, null,
                    It.IsAny<Expression<Func<BookBorrowingRequest, object>>>(),
                    It.IsAny< Expression<Func<BookBorrowingRequest, object>>>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _requestService.GetBookBorrowingRequestsPaginatedAsync(pageIndex, pageSize);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PageIndex, Is.EqualTo(pageIndex));
            Assert.That(result.PageSize, Is.EqualTo(pageSize));
            Assert.That(result.Items?.Count(), Is.EqualTo(2));
            Assert.That(result.Items.ElementAt(0).Status, Is.EqualTo(RequestStatus.Waiting));
        }

        #endregion

        #region GetBookBorrowingRequestByIdAsync Tests

        [Test]
        public async Task GetBookBorrowingRequestByIdAsync_WithExistingRequest_ReturnsRequestDTO()
        {
            // Arrange
            var requestId = 1;
            var request = new BookBorrowingRequest { Id = requestId, Status = RequestStatus.Waiting };

            _mockRequestRepository.Setup(r => r.GetByIdAsync(requestId))
                .ReturnsAsync(request);

            // Act
            var result = await _requestService.GetBookBorrowingRequestByIdAsync(requestId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(requestId));
            Assert.That(result.Status, Is.EqualTo(RequestStatus.Waiting));
        }

        [Test]
        public void GetBookBorrowingRequestByIdAsync_WithNonExistingRequest_ThrowsNotFoundException()
        {
            // Arrange
            var requestId = 999;
            _mockRequestRepository.Setup(r => r.GetByIdAsync(requestId))
                .ReturnsAsync(null as BookBorrowingRequest);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _requestService.GetBookBorrowingRequestByIdAsync(requestId));
            Assert.That(ex.Message, Is.EqualTo($"Book borrowing request with ID {requestId} not found."));
        }

        #endregion

        #region GetBookBorrowingRequestsThisMonthAsync Tests

        [Test]
        public async Task GetBookBorrowingRequestsThisMonthAsync_ReturnsRequestsForCurrentMonth()
        {
            // Arrange
            var requestorId = 1;
            var requests = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { Id = 1, RequestorId = requestorId, RequestedDate = DateTime.UtcNow },
                new BookBorrowingRequest { Id = 2, RequestorId = requestorId, RequestedDate = DateTime.UtcNow }
            };

            _mockRequestRepository.Setup(r => r.GetFiltersAsync(
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>(),
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>()))
                .ReturnsAsync(requests);

            // Act
            var result = await _requestService.GetBookBorrowingRequestsThisMonthAsync(requestorId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        #endregion

        #region GetBorrowingRequestsByRequestorId Tests

        [Test]
        public async Task GetBorrowingRequestsByRequestorId_ReturnsPagedResponse()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var totalRecords = 2;
            var requestorId = 1;
            var requests = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { Id = 1, RequestorId = requestorId },
                new BookBorrowingRequest { Id = 2, RequestorId = requestorId }
            };

            var pagedResult = new PagedResult<BookBorrowingRequest>(requests, pageIndex, pageSize, totalRecords);

            _mockRequestRepository.Setup(r => r.GetPagedAsync(
                    pageIndex, pageSize,
                    It.IsAny<Expression<Func<BookBorrowingRequest, bool>>>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _requestService.GetBorrowingRequestsByRequestorId(pageIndex, pageSize, requestorId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PageIndex, Is.EqualTo(pageIndex));
            Assert.That(result.PageSize, Is.EqualTo(pageSize));
            Assert.That(result.Items?.Count(), Is.EqualTo(2));
        }

        #endregion

        #region UpdateBookBorrowingRequestAsync Tests

        [Test]
        public async Task UpdateBookBorrowingRequestAsync_WithValidData_UpdatesRequest()
        {
            // Arrange
            var requestId = 1;
            var approverId = 2;
            var updateDto = new BorrowingRequestToUpdateDTO { Status = RequestStatus.Approved };
            var existingRequest = new BookBorrowingRequest { Id = requestId, Status = RequestStatus.Waiting };

            _mockRequestRepository.Setup(r => r.GetByIdAsync(requestId))
                .ReturnsAsync(existingRequest);

            _mockRequestRepository.Setup(r => r.UpdateAsync(It.IsAny<BookBorrowingRequest>()))
                .ReturnsAsync((BookBorrowingRequest r) => r);

            // Act
            var result = await _requestService.UpdateBookBorrowingRequestAsync(
                requestId, approverId, updateDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RequestStatus.Approved));
            Assert.That(result.ApproverId, Is.EqualTo(approverId));
            _mockRequestRepository.Verify(r => r.UpdateAsync(It.IsAny<BookBorrowingRequest>()), Times.Once);
        }

        [Test]
        public void UpdateBookBorrowingRequestAsync_WithNonExistingRequest_ThrowsNotFoundException()
        {
            // Arrange
            var requestId = 999;
            var approverId = 2;
            var updateDto = new BorrowingRequestToUpdateDTO { Status = RequestStatus.Approved };

            _mockRequestRepository.Setup(r => r.GetByIdAsync(requestId))
                .ReturnsAsync(null as BookBorrowingRequest);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _requestService.UpdateBookBorrowingRequestAsync(requestId, approverId, updateDto));
            Assert.That(ex.Message, Is.EqualTo($"Book borrowing request with ID {requestId} not found."));
        }

        #endregion
    }
}
