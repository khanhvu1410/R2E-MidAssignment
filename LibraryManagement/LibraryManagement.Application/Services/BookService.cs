using FluentValidation;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<BookBorrowingRequestDetails> _bookBorrowingRequestDetails;
        private readonly IValidator<BookToAddDTO> _bookToAddDTOValidator;
        private readonly IValidator<BookDTO> _bookDTOValidator;

        public BookService(IGenericRepository<Book> bookRepository, 
            IValidator<BookToAddDTO> bookToAddDTOvalidator, 
            IValidator<BookDTO> bookDTOValidator,
            IGenericRepository<BookBorrowingRequestDetails> bookBorrowingRequestDetails)
        {
            _bookRepository = bookRepository;
            _bookBorrowingRequestDetails = bookBorrowingRequestDetails;
            _bookToAddDTOValidator = bookToAddDTOvalidator;
            _bookDTOValidator = bookDTOValidator;
        }

        public async Task<BookDTO> AddBookAsync(BookToAddDTO bookToAddDTO)
        {
            var result = await _bookToAddDTOValidator.ValidateAsync(bookToAddDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            var addedBook = await _bookRepository.AddAsync(bookToAddDTO.ToBook());
            return addedBook.ToBookDTO();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new NotFoundException($"Book with ID {id} not found.");
            }

            // Check if this book is being borrowed
            var query = _bookBorrowingRequestDetails.GetQueryable();
            var requestDetailsByBookId = query.Where(rd => rd.BookId == book.Id);
            if (requestDetailsByBookId.Any())
            {
                throw new BadRequestException($"Book with ID {id} cannot be deleted.");
            }

            await _bookRepository.DeleteAsync(id);
        }

        public async Task<PagedResponse<BookDTO>> GetBooksPaginatedAsync(int pageIndex, int pageSize)
        {
            var pageResult = await _bookRepository.GetPagedAsync(pageIndex, pageSize);
            var pageResponse = new PagedResponse<BookDTO>
            {
                Items = pageResult.Items?.Select(b => b.ToBookDTO()).ToList(),
                PageIndex = pageResult.PageIndex,
                TotalPages = pageResult.TotalPages,
                HasPreviousPage = pageResult.HasPreviousPage,
                HasNextPage = pageResult.HasNextPage,
            };
            return pageResponse;
        }

        public async Task<BookDTO> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new NotFoundException($"Book with ID {id} not found.");
            } 
            return book.ToBookDTO();
        }
        public async Task<BookDTO> UpdateBookAsync(BookDTO bookDTO)
        {
            var result = await _bookDTOValidator.ValidateAsync(bookDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            var book = await _bookRepository.GetByIdAsync(bookDTO.Id);
            if (book == null)
            {
                throw new NotFoundException($"Book with ID {bookDTO.Id} not found.");
            }
            var updatedBook = await _bookRepository.UpdateAsync(book);
            return updatedBook.ToBookDTO();
        }
    }
}
