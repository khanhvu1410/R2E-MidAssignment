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
    public class BorrowingRequestsController : ControllerBase
    {
        private readonly IBorrowingRequestService _borrowingRequestService;

        public BorrowingRequestsController(IBorrowingRequestService borrowingRequestService)
        {
            _borrowingRequestService = borrowingRequestService;
        }

        [HttpPost]
        [Authorize(Roles = "NormalUser")]
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> CreateBorrowingRequest(IEnumerable<RequestDetailsToAddDTO> borrowingRequestDetailsDTOs)
        {
            var requestorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var addedBorrowingRequest = await _borrowingRequestService.AddBorrowingRequestAsync(requestorId, borrowingRequestDetailsDTOs);
            return CreatedAtAction(nameof(GetBookBorrowingRequestById), new { id = addedBorrowingRequest.Id }, addedBorrowingRequest);
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<PagedResponse<BorrowingRequestToReturnDTO>>> GetBorrowingRequests(int pageIndex, int pageSize)
        {
            var pagedResponse = await _borrowingRequestService.GetBorrowingRequestsPaginatedAsync(pageIndex, pageSize);
            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> GetBookBorrowingRequestById(int id)
        {
            var bookBorrowingRequest = await _borrowingRequestService.GetBorrowingRequestByIdAsync(id);
            return Ok(bookBorrowingRequest);
        }

        [HttpGet("ThisMonth")]
        [Authorize(Roles = "NormalUser")]
        public async Task<ActionResult<IEnumerable<BorrowingRequestToReturnDTO>>> GetBorrowingRequestsThisMonth()
        {
            var requestorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var borrowingRequests = await _borrowingRequestService.GetBorrowingRequestsThisMonthAsync(requestorId);
            return Ok(borrowingRequests);
        }

        [HttpGet("GetByRequestorId")]
        [Authorize(Roles = "NormalUser")]
        public async Task<ActionResult<PagedResponse<BorrowingRequestToReturnDTO>>> GetBorrowingRequestsByRequestorId(int pageIndex, int pageSize)
        {
            var requestorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var pagedResponse = await _borrowingRequestService.GetBorrowingRequestsByRequestorId(pageIndex, pageSize, requestorId);
            return Ok(pagedResponse);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<BorrowingRequestToReturnDTO>> UpdateBookBorrowingRequest(int id, BorrowingRequestToUpdateDTO borrowingRequestToUpdateDTO)
        {
            var approverId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var updatedBorrowingRequest = await _borrowingRequestService.UpdateBorrowingRequestAsync(id, approverId, borrowingRequestToUpdateDTO);
            return Ok(updatedBorrowingRequest);
        }
    }
}
