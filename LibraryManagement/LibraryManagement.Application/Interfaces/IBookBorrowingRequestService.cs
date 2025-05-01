using LibraryManagement.Application.DTOs.BorrowingRequest;
using LibraryManagement.Application.DTOs.RequestDetails;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookBorrowingRequestService
    {
        public Task<BorrowingRequestToReturnDTO> AddBookBorrowingRequestAsync(int requestorId, IEnumerable<RequestDetailsToAddDTO> bookBorrowingRequestDetailsDTOs);

        public Task<IEnumerable<BorrowingRequestToReturnDTO>> GetAllBookBorrowingRequestsAsync();

        public Task<BorrowingRequestToReturnDTO> GetBookBorrowingRequestByIdAsync(int id);

        public Task<IEnumerable<BorrowingRequestToReturnDTO>> GetBorrowingRequestsByRequestorId(int requestorId);

        public Task<BorrowingRequestToReturnDTO> UpdateBookBorrowingRequestAsync(int id, int approverId, BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO);
    }
}
