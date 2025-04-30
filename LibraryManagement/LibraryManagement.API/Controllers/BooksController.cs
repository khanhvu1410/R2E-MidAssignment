using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<BookToReturnDTO>> CreateBook(BookToAddDTO bookToAddDTO)
        {
            var createdBook = await _bookService.AddBookAsync(bookToAddDTO);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<BookToReturnDTO>>> GetBooks(int pageIndex, int pageSize)
        {
            var pagedResponse = await _bookService.GetBooksPaginatedAsync(pageIndex, pageSize);
            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookToReturnDTO>> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return Ok(book);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<BookToReturnDTO>> UpdateBook(int id, BookToUpdateDTO bookToUpdateDTO)
        {
            var updatedBook = await _bookService.UpdateBookAsync(id, bookToUpdateDTO);
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}
