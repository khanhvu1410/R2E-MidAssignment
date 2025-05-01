using System.Security.Claims;
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
        [Authorize(Roles = "NormalUser")]
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> CreateBookBorrowingRequest(IEnumerable<RequestDetailsToAddDTO> bookBorrowingRequestDetailsDTOs)
        {
            var requestorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var addedBookBorrowingRequest = await _bookBorrowingRequestService.AddBookBorrowingRequestAsync(requestorId, bookBorrowingRequestDetailsDTOs);
            return CreatedAtAction(nameof(GetBookBorrowingRequestById), new { id =  addedBookBorrowingRequest.Id }, addedBookBorrowingRequest);
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
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

        [HttpPatch("{borrowingRequestId}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> UpdateBookBorrowingRequest(int borrowingRequestId, BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO)
        {
            var approverId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var updatedBorrowingRequest = await _bookBorrowingRequestService.UpdateBookBorrowingRequestAsync(borrowingRequestId, approverId, borrowingRequestToUpdateDTO);
            return Ok(updatedBorrowingRequest);
        }
    }
}
