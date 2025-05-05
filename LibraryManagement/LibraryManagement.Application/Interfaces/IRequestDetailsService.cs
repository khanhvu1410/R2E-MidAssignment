using LibraryManagement.Application.DTOs.RequestDetails;

namespace LibraryManagement.Application.Interfaces
{
    public interface IRequestDetailsService
    {
        public Task<IEnumerable<RequestDetailsToReturnDTO>> GetRequestDetailsByBorrowingRequestId(int borrowingRequestId);
    }
}
