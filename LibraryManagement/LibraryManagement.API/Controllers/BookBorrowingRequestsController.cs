using LibraryManagement.Application.DTOs.BorrowingRequest;
using LibraryManagement.Application.DTOs.RequestDetails;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BookBorrowingRequestsController : ControllerBase
    {
        private readonly IBookBorrowingRequestService _bookBorrowingRequestService;

        public BookBorrowingRequestsController(IBookBorrowingRequestService bookBorrowingRequestService)
        {
            _bookBorrowingRequestService = bookBorrowingRequestService;
        }

        [HttpPost]
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> CreateBookBorrowingRequest(IEnumerable<RequestDetailsToAddDTO> bookBorrowingRequestDetailsDTOs)
        {
            var addedBookBorrowingRequest = await _bookBorrowingRequestService.AddBookBorrowingRequestAsync(bookBorrowingRequestDetailsDTOs);
            return CreatedAtAction(nameof(GetBookBorrowingRequestById), new { id =  addedBookBorrowingRequest.Id }, addedBookBorrowingRequest);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingRequestToReturnDTO>>> GetAllBookBorrowingRequests()
        {
            var bookBorrowingRequests = await _bookBorrowingRequestService.GetAllBookBorrowingRequestsAsync();
            return Ok(bookBorrowingRequests);
        }

        [HttpGet("{id}")] 
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> GetBookBorrowingRequestById(int id)
        {
            var bookBorrowingRequest = await _bookBorrowingRequestService.GetBookBorrowingRequestByIdAsync(id);
            return Ok(bookBorrowingRequest);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> UpdateBookBorrowingRequest(int id, BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO)
        {
            var updatedBorrowingRequest = await _bookBorrowingRequestService.UpdateBookBorrowingRequestAsync(id, borrowingRequestToUpdateDTO);
            return Ok(updatedBorrowingRequest);
        }
    }
}
