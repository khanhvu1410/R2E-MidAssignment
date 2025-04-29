using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookBorrowingRequestService
    {
        public Task<BorrowingRequestDTO> AddBookBorrowingRequestAsync(IEnumerable<RequestDetailsDTO> bookBorrowingRequestDetailsDTOs);

        public Task<IEnumerable<BorrowingRequestDTO>> GetAllBookBorrowingRequestsAsync();

        public Task<BorrowingRequestDTO> GetBookBorrowingRequestByIdAsync(int id);

        public Task<BorrowingRequestDTO> UpdateBookBorrowingRequestAsync(BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO);
    }
}
