using FluentValidation;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Book;
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
        private readonly IGenericRepository<BookBorrowingRequestDetails> _requestDetailsRepository;
        private readonly IValidator<BookToAddDTO> _bookToAddDTOValidator;
        private readonly IValidator<BookToUpdateDTO> _bookToUpdateDTOValidator;

        public BookService(IGenericRepository<Book> bookRepository,
            IGenericRepository<BookBorrowingRequestDetails> requestDetailsRepository,
            IValidator<BookToAddDTO> bookToAddDTOvalidator, 
            IValidator<BookToUpdateDTO> bookToUpdateDTOValidator
        )
        {
            _bookRepository = bookRepository;
            _requestDetailsRepository = requestDetailsRepository;
            _bookToAddDTOValidator = bookToAddDTOvalidator;
            _bookToUpdateDTOValidator = bookToUpdateDTOValidator;
        }

        public async Task<BookToReturnDTO> AddBookAsync(BookToAddDTO bookToAddDTO)
        {
            var result = await _bookToAddDTOValidator.ValidateAsync(bookToAddDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            var addedBook = await _bookRepository.AddAsync(bookToAddDTO.ToBook());
            return addedBook.ToBookToReturnDTO();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new NotFoundException($"Book with ID {id} not found.");
            }

            // Check if this book is being borrowed
            var requestDetailsExists = await _requestDetailsRepository.ExistsAsync(rd => rd.BookId == book.Id);
            if (requestDetailsExists)
            {
                throw new BadRequestException($"Book with ID {id} cannot be deleted.");
            }

            await _bookRepository.DeleteAsync(id);
        }

        public async Task<PagedResponse<BookToReturnDTO>> GetBooksPaginatedAsync(int pageIndex, int pageSize)
        {
            var pagedResult = await _bookRepository.GetPagedAsync(pageIndex, pageSize, null, b => b.Category);
            var pagedResponse = new PagedResponse<BookToReturnDTO>
            {
                Items = pagedResult.Items.Select(b => b.ToBookToReturnDTO()).ToList(),
                PageIndex = pagedResult.PageIndex,
                PageSize = pagedResult.PageSize,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = pagedResult.TotalPages,
                HasPreviousPage = pagedResult.HasPreviousPage,
                HasNextPage = pagedResult.HasNextPage,
            };
            return pagedResponse;
        }

        public async Task<BookToReturnDTO> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id, b => b.Category);
            if (book == null)
            {
                throw new NotFoundException($"Book with ID {id} not found.");
            } 
            return book.ToBookToReturnDTO();
        }
        public async Task<BookToReturnDTO> UpdateBookAsync(int id, BookToUpdateDTO bookToUpdateDTO)
        {
            var result = await _bookToUpdateDTOValidator.ValidateAsync(bookToUpdateDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new NotFoundException($"Book with ID {id} not found.");
            }

            book.Title = bookToUpdateDTO.Title;
            book.Author = bookToUpdateDTO.Author;            
            book.ISBN = bookToUpdateDTO.ISBN;           
            book.PublicationYear = bookToUpdateDTO.PublicationYear;
            book.Description = bookToUpdateDTO.Description;
            book.Quantity = bookToUpdateDTO.Quantity;
            book.CategoryId = bookToUpdateDTO.CategoryId;

            var updatedBook = await _bookRepository.UpdateAsync(book);
            return updatedBook.ToBookToReturnDTO();
        }
    }
}
