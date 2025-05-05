using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.BorrowingRequest;
using LibraryManagement.Application.DTOs.RequestDetails;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBorrowingRequestService
    {
        public Task<BorrowingRequestToReturnDTO> AddBorrowingRequestAsync(int requestorId, IEnumerable<RequestDetailsToAddDTO> bookBorrowingRequestDetailsDTOs);

        public Task<PagedResponse<BorrowingRequestToReturnDTO>> GetBorrowingRequestsPaginatedAsync(int pageIndex, int pageSize);

        public Task<BorrowingRequestToReturnDTO> GetBorrowingRequestByIdAsync(int id);

        public Task<IEnumerable<BorrowingRequestToReturnDTO>> GetBorrowingRequestsThisMonthAsync(int requestorId);

        public Task<PagedResponse<BorrowingRequestToReturnDTO>> GetBorrowingRequestsByRequestorId(int pageIndex, int pageSize, int requestorId);

        public Task<BorrowingRequestToReturnDTO> UpdateBorrowingRequestAsync(int id, int approverId, BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO);
    }
}
