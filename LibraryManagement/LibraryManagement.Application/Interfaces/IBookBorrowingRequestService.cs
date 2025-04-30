using LibraryManagement.Application.DTOs.BorrowingRequest;
using LibraryManagement.Application.DTOs.RequestDetails;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookBorrowingRequestService
    {
        public Task<BorrowingRequestToReturnDTO> AddBookBorrowingRequestAsync(IEnumerable<RequestDetailsToAddDTO> bookBorrowingRequestDetailsDTOs);

        public Task<IEnumerable<BorrowingRequestToReturnDTO>> GetAllBookBorrowingRequestsAsync();

        public Task<BorrowingRequestToReturnDTO> GetBookBorrowingRequestByIdAsync(int id);

        public Task<BorrowingRequestToReturnDTO> UpdateBookBorrowingRequestAsync(int id, BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO);
    }
}
