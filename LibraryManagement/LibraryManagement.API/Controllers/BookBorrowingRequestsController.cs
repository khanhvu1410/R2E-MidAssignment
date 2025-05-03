using System.Security.Claims;
using LibraryManagement.Application.Common;
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
            return CreatedAtAction(nameof(GetBookBorrowingRequestById), new { id = addedBookBorrowingRequest.Id }, addedBookBorrowingRequest);
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<PagedResponse<BorrowingRequestToReturnDTO>>> GetBookBorrowingRequests(int pageIndex, int pageSize)
        {
            var pagedResponse = await _bookBorrowingRequestService.GetBookBorrowingRequestsPaginatedAsync(pageIndex, pageSize);
            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> GetBookBorrowingRequestById(int id)
        {
            var bookBorrowingRequest = await _bookBorrowingRequestService.GetBookBorrowingRequestByIdAsync(id);
            return Ok(bookBorrowingRequest);
        }

        [HttpGet("ThisMonth")]
        [Authorize(Roles = "NormalUser")]
        public async Task<ActionResult<IEnumerable<BorrowingRequestToReturnDTO>>> GetBorrowingRequestsThisMonth()
        {
            var requestorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var borrowingRequests = await _bookBorrowingRequestService.GetBookBorrowingRequestsThisMonthAsync(requestorId);
            return Ok(borrowingRequests);
        }

        [HttpGet("GetByRequestorId")]
        [Authorize(Roles = "NormalUser")]
        public async Task<ActionResult<PagedResponse<BorrowingRequestToReturnDTO>>> GetBorrowingRequestsByRequestorId(int pageIndex, int pageSize)
        {
            var requestorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var pagedResponse = await _bookBorrowingRequestService.GetBorrowingRequestsByRequestorId(pageIndex, pageSize, requestorId);
            return Ok(pagedResponse);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> UpdateBookBorrowingRequest(int id, BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO)
        {
            var approverId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var updatedBorrowingRequest = await _bookBorrowingRequestService.UpdateBookBorrowingRequestAsync(id, approverId, borrowingRequestToUpdateDTO);
            return Ok(updatedBorrowingRequest);
        }
    }
}
