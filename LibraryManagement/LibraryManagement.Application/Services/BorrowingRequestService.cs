using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.BorrowingRequest;
using LibraryManagement.Application.DTOs.RequestDetails;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class BorrowingRequestService : IBorrowingRequestService
    {
        private readonly IGenericRepository<BookBorrowingRequest> _borrowingRequestRepository;
        private readonly IGenericRepository<BookBorrowingRequestDetails> _requestDetailsRepository;
        private readonly IGenericRepository<Book> _bookRepository;

        public BorrowingRequestService(IGenericRepository<BookBorrowingRequest> borrowingRequestRepository, 
            IGenericRepository<BookBorrowingRequestDetails> requestDetailsRepository,
            IGenericRepository<Book> bookRepository)
        {
            _borrowingRequestRepository = borrowingRequestRepository;
            _requestDetailsRepository = requestDetailsRepository;
            _bookRepository = bookRepository;
        }

        public async Task<BorrowingRequestToReturnDTO> AddBorrowingRequestAsync(int requestorId, IEnumerable<RequestDetailsToAddDTO> requestDetailsDTOs)
        {
            using var transaction = await _borrowingRequestRepository.BeginTransactionAsync();
            try
            {
                // Limit up to 3 request per month
                var filteredBorrowingRequests = await GetBorrowingRequestsThisMonthAsync(requestorId);
                if (filteredBorrowingRequests.Count() >= 3)
                {
                    throw new BadRequestException("Maximum 3 borrowing requests per month.");
                }

                // Limit up to 5 books per request
                int count = requestDetailsDTOs.Count();               
                if (count > 5)
                {
                    throw new BadRequestException("Maximum 5 books can be requested at a time.");
                }

                // Pre-load all books in one query
                var bookIds = requestDetailsDTOs.Select(x => x.BookId).Distinct();
                var books = await _bookRepository.GetByIdsAsync(bookIds);
                var booksDict = books.ToDictionary(b => b.Id);

                foreach (var requestDetailsDTO in requestDetailsDTOs)
                {
                    // Validate all book before processing
                    if (!booksDict.TryGetValue(requestDetailsDTO.BookId, out var book))
                    {
                        throw new NotFoundException($"Book with ID {requestDetailsDTO.BookId} not found.");
                    }

                    // Check availability of book
                    if (book.Quantity <= 0)
                    {
                        throw new BadRequestException($"Book with ID {requestDetailsDTO.BookId} is unavailable.");
                    }
                }

                // Create borrowing request
                var borrowingRequest = new BookBorrowingRequest
                {
                    RequestorId = requestorId, 
                    RequestedDate = DateTime.UtcNow,
                    Status = RequestStatus.Waiting
                };
                var addedBorrowingRequest = await _borrowingRequestRepository.AddAsync(borrowingRequest);

                // Process all request details
                foreach (var requestDetailsDTO in requestDetailsDTOs)
                {
                    var book = booksDict[requestDetailsDTO.BookId];
                    book.Quantity -= 1;

                    var requestDetails = new BookBorrowingRequestDetails
                    {
                        BookBorrowingRequestId = addedBorrowingRequest.Id,
                        BookId = requestDetailsDTO.BookId,
                    };

                    await _bookRepository.UpdateAsync(book);
                    await _requestDetailsRepository.AddAsync(requestDetails);
                }

                await transaction.CommitAsync();
                return addedBorrowingRequest.ToBookBorrowingRequestToReturnDTO();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }     
        }

        public async Task<PagedResponse<BorrowingRequestToReturnDTO>> GetBorrowingRequestsPaginatedAsync(int pageIndex, int pageSize)
        {
            var pagedResult = await _borrowingRequestRepository.GetPagedAsync(pageIndex, pageSize, null, br => br.Approver, br => br.Requestor);
            var pagedResponse = new PagedResponse<BorrowingRequestToReturnDTO>
            {
                Items = pagedResult.Items?
                    .Select(br => br.ToBookBorrowingRequestToReturnDTO())
                    .ToList() ?? default!,
                PageIndex = pagedResult.PageIndex,
                PageSize = pagedResult.PageSize,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = pagedResult.TotalPages,
                HasPreviousPage = pagedResult.HasPreviousPage,
                HasNextPage = pagedResult.HasNextPage,
            };
            return pagedResponse;
        }

        public async Task<BorrowingRequestToReturnDTO> GetBorrowingRequestByIdAsync(int id)
        {
            var bookBorrowingRequest = await _borrowingRequestRepository.GetByIdAsync(id);
            if (bookBorrowingRequest == null)
            {
                throw new NotFoundException($"Book borrowing request with ID {id} not found.");
            }
            return bookBorrowingRequest.ToBookBorrowingRequestToReturnDTO();
        }

        public async Task<IEnumerable<BorrowingRequestToReturnDTO>> GetBorrowingRequestsThisMonthAsync(int requestorId)
        {
            var filteredBorrowingRequests = await _borrowingRequestRepository.GetFiltersAsync(
                br => br.RequestorId == requestorId,
                br => br.RequestedDate.Month == DateTime.UtcNow.Month,
                br => br.RequestedDate.Year == DateTime.UtcNow.Year
            );
            return filteredBorrowingRequests.Select(br => br.ToBookBorrowingRequestToReturnDTO());
        }

        public async Task<PagedResponse<BorrowingRequestToReturnDTO>> GetBorrowingRequestsByRequestorId(int pageIndex, int pageSize, int requestorId)
        {
            var pagedResult = await _borrowingRequestRepository.GetPagedAsync(pageIndex, pageSize, br => br.RequestorId == requestorId);
            var pagedResponse = new PagedResponse<BorrowingRequestToReturnDTO>
            {
                Items = pagedResult.Items?.Select(br => br.ToBookBorrowingRequestToReturnDTO()).ToList() ?? default!,
                PageIndex = pagedResult.PageIndex,
                PageSize = pagedResult.PageSize,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = pagedResult.TotalPages,
                HasPreviousPage = pagedResult.HasPreviousPage,
                HasNextPage = pagedResult.HasNextPage,
            };
            return pagedResponse;
        }

        public async Task<BorrowingRequestToReturnDTO> UpdateBorrowingRequestAsync(int id, int approverId, BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO)
        {
            var borrowingRequest = await _borrowingRequestRepository.GetByIdAsync(id);
            if (borrowingRequest == null)
            {
                throw new NotFoundException($"Book borrowing request with ID {id} not found.");
            }
            
            borrowingRequest.ApproverId = approverId;
            borrowingRequest.Status = borrowingRequestToUpdateDTO.Status;

            var updatedBorrowingRequest = await _borrowingRequestRepository.UpdateAsync(borrowingRequest);
            return updatedBorrowingRequest.ToBookBorrowingRequestToReturnDTO();
        }
    }
}
