using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class BookBorrowingRequestService : IBookBorrowingRequestService
    {
        private readonly IGenericRepository<BookBorrowingRequest> _bookBorrowingRequestRepository;
        private readonly IGenericRepository<BookBorrowingRequestDetails> _bookBorrowingRequestDetailsRepository;
        private readonly IGenericRepository<Book> _bookRepository;

        public BookBorrowingRequestService(IGenericRepository<BookBorrowingRequest> bookBorrowingRequestRepository, 
            IGenericRepository<BookBorrowingRequestDetails> bookBorrowingRequestDetailsRepository,
            IGenericRepository<Book> bookRepository)
        {
            _bookBorrowingRequestRepository = bookBorrowingRequestRepository;
            _bookBorrowingRequestDetailsRepository = bookBorrowingRequestDetailsRepository;
            _bookRepository = bookRepository;
        }

        public async Task<BorrowingRequestDTO> AddBookBorrowingRequestAsync(IEnumerable<RequestDetailsDTO> requestDetailsDTOs)
        {
            using var transaction = await _bookBorrowingRequestRepository.BeginTransactionAsync();
            try
            {
                int requestorId = 2; // TODO: Replace with actual requestor ID from auth context

                // Pre-load all books in one query
                var bookIds = requestDetailsDTOs.Select(x => x.BookId).Distinct();
                var books = await _bookRepository.GetByIdsAsync(bookIds);
                var booksDict = books.ToDictionary(b => b.Id);
                int count = 0;

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

                    count++;
                }

                // Limit up to 5 books per request
                if (count > 5)
                {
                    throw new BadRequestException("Maximum 5 books can be requested at a time");
                }

                // Create borrowing request
                var borrowingRequest = new BookBorrowingRequest
                {
                    RequestorId = requestorId, 
                    RequestedDate = DateTime.UtcNow,
                    Status = RequestStatus.Waiting
                };
                var addedBorrowingRequest = await _bookBorrowingRequestRepository.AddAsync(borrowingRequest);

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
                    await _bookBorrowingRequestDetailsRepository.AddAsync(requestDetails);
                }

                await transaction.CommitAsync();
                return addedBorrowingRequest.ToBookBorrowingRequestDTO();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }     
        }

        public async Task<IEnumerable<BorrowingRequestDTO>> GetAllBookBorrowingRequestsAsync()
        {
            var bookBorrowingRequests = await _bookBorrowingRequestRepository.GetAllAsync();
            return bookBorrowingRequests.Select(bbr => bbr.ToBookBorrowingRequestDTO());
        }

        public async Task<BorrowingRequestDTO> GetBookBorrowingRequestByIdAsync(int id)
        {
            var bookBorrowingRequest = await _bookBorrowingRequestRepository.GetByIdAsync(id);
            if (bookBorrowingRequest == null)
            {
                throw new NotFoundException($"Book borrowing request with ID {id} not found.");
            }
            return bookBorrowingRequest.ToBookBorrowingRequestDTO();
        }

        public async Task<BorrowingRequestDTO> UpdateBookBorrowingRequestAsync(BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO)
        {
            int approverId = 1; // TODO: Replace with actual approver ID from auth context
            
            var borrowingRequest = await _bookBorrowingRequestRepository.GetByIdAsync(borrowingRequestToUpdateDTO.Id);
            if (borrowingRequest == null)
            {
                throw new NotFoundException($"Book borrowing request with ID {borrowingRequestToUpdateDTO.Id} not found.");
            }
            
            borrowingRequest.ApproverId = approverId;
            borrowingRequest.Status = borrowingRequestToUpdateDTO.Status;

            var updatedBorrowingRequest = await _bookBorrowingRequestRepository.UpdateAsync(borrowingRequest);
            return updatedBorrowingRequest.ToBookBorrowingRequestDTO();
        }
    }
}
