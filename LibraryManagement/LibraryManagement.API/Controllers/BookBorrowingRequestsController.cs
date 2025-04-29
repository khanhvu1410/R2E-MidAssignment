using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookBorrowingRequestsController : ControllerBase
    {
        private readonly IBookBorrowingRequestService _bookBorrowingRequestService;

        public BookBorrowingRequestsController(IBookBorrowingRequestService bookBorrowingRequestService)
        {
            _bookBorrowingRequestService = bookBorrowingRequestService;
        }

        [HttpPost]
        public async Task<ActionResult<BorrowingRequestDTO>> CreateBookBorrowingRequest(IEnumerable<RequestDetailsToAddDTO> bookBorrowingRequestDetailsDTOs)
        {
            var addedBookBorrowingRequest = await _bookBorrowingRequestService.AddBookBorrowingRequestAsync(bookBorrowingRequestDetailsDTOs);
            return CreatedAtAction(nameof(GetBookBorrowingRequestById), new { id =  addedBookBorrowingRequest.Id }, addedBookBorrowingRequest);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingRequestDTO>>> GetAllBookBorrowingRequests()
        {
            var bookBorrowingRequests = await _bookBorrowingRequestService.GetAllBookBorrowingRequestsAsync();
            return Ok(bookBorrowingRequests);
        }

        [HttpGet("{id}")] 
        public async Task<ActionResult<BorrowingRequestDTO>> GetBookBorrowingRequestById(int id)
        {
            var bookBorrowingRequest = await _bookBorrowingRequestService.GetBookBorrowingRequestByIdAsync(id);
            return Ok(bookBorrowingRequest);
        }

        [HttpPatch]
        public async Task<ActionResult<BorrowingRequestDTO>> UpdateBookBorrowingRequest(BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO)
        {
            var updatedBorrowingRequest = await _bookBorrowingRequestService.UpdateBookBorrowingRequestAsync(borrowingRequestToUpdateDTO);
            return Ok(updatedBorrowingRequest);
        }
    }
}
