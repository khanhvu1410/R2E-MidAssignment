using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Mappers;

namespace LibraryManagement.Application.Tests.Mappers
{
    [TestFixture]
    internal class BorrowingRequestMapperTests
    {
        [Test]
        public void ToBookBorrowingRequestToReturnDTO_WithCompleteRequest_ReturnsCorrectDTO()
        {
            // Arrange
            var request = new BookBorrowingRequest
            {
                Id = 1,
                RequestorId = 10,
                ApproverId = 20,
                Requestor = new User
                {
                    Id = 10,
                    Username = "requester",
                },
                Approver = new User
                {
                    Id = 20,
                    Username = "approver",
                },
                RequestedDate = new DateTime(2023, 1, 1),
                Status = RequestStatus.Approved
            };

            // Act
            var result = request.ToBookBorrowingRequestToReturnDTO();

            // Assert           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(request.Id));
                Assert.That(result.RequestorId, Is.EqualTo(request.RequestorId));
                Assert.That(result.ApproverId, Is.EqualTo(request.ApproverId));
                Assert.That(result.RequestorUsername, Is.EqualTo(request.Requestor.Username));
                Assert.That(result.ApproverUsername, Is.EqualTo(request.Approver.Username));
                Assert.That(result.RequestedDate, Is.EqualTo(request.RequestedDate));
                Assert.That(result.Status, Is.EqualTo(request.Status));
            });
        }

        [Test]
        public void ToBookBorrowingRequestToReturnDTO_WithNullApprover_ReturnsDTOWithNullApproverUsername()
        {
            // Arrange
            var request = new BookBorrowingRequest
            {
                Id = 2,
                RequestorId = 11,
                ApproverId = null,
                Approver = null,
                Requestor = new User
                {
                    Id = 10,
                    Username = "requester",
                },
                RequestedDate = new DateTime(2023, 1, 2),
                Status = RequestStatus.Waiting
            };

            // Act
            var result = request.ToBookBorrowingRequestToReturnDTO();

            // Assert          
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.ApproverUsername, Is.Null);
            });
        }

        [Test]
        public void ToBookBorrowingRequestToReturnDTO_WithNullRequest_ReturnNullOutput()
        {
            // Arrange
            BookBorrowingRequest? request = null;

            // Act & Assert
            Assert.That(request?.ToBookBorrowingRequestToReturnDTO(), Is.Null);
        }

        [Test]
        public void ToBookBorrowingRequestToReturnDTO_WithAllStatusValues_ReturnsCorrectDTO()
        {
            // Test all enum values to ensure proper mapping
            foreach (RequestStatus status in Enum.GetValues(typeof(RequestStatus)))
            {
                // Arrange
                var request = new BookBorrowingRequest
                {
                    Id = 2,
                    RequestorId = 11,
                    RequestedDate = new DateTime(2023, 1, 2),
                    Requestor = new User
                    {
                        Id = 10,
                        Username = "requester",
                    },
                    Status = status
                };

                // Act
                var result = request.ToBookBorrowingRequestToReturnDTO();

                // Assert
                Assert.That(result.Status, Is.EqualTo(status));
            }
        }
    }
}
