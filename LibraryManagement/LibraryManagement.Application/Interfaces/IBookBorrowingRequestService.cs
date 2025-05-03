using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.BorrowingRequest;
using LibraryManagement.Application.DTOs.RequestDetails;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookBorrowingRequestService
    {
        public Task<BorrowingRequestToReturnDTO> AddBookBorrowingRequestAsync(int requestorId, IEnumerable<RequestDetailsToAddDTO> bookBorrowingRequestDetailsDTOs);

        public Task<PagedResponse<BorrowingRequestToReturnDTO>> GetBookBorrowingRequestsPaginatedAsync(int pageIndex, int pageSize);

        public Task<BorrowingRequestToReturnDTO> GetBookBorrowingRequestByIdAsync(int id);

        public Task<IEnumerable<BorrowingRequestToReturnDTO>> GetBookBorrowingRequestsThisMonthAsync(int requestorId);

        public Task<PagedResponse<BorrowingRequestToReturnDTO>> GetBorrowingRequestsByRequestorId(int pageIndex, int pageSize, int requestorId);

        public Task<BorrowingRequestToReturnDTO> UpdateBookBorrowingRequestAsync(int id, int approverId, BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO);
    }
}
