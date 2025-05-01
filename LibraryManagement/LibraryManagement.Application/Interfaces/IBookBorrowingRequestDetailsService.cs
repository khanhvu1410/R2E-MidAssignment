using LibraryManagement.Application.DTOs.RequestDetails;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookBorrowingRequestDetailsService
    {
        public Task<IEnumerable<RequestDetailsToReturnDTO>> GetRequestDetailsByBorrowingRequestId(int borrowingRequestId);
    }
}
