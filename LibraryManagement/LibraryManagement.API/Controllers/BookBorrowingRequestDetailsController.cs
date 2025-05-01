using LibraryManagement.Application.DTOs.RequestDetails;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BookBorrowingRequestDetailsController : ControllerBase
    {
        private readonly IBookBorrowingRequestDetailsService _bookBorrowingRequestDetailsService;

        public BookBorrowingRequestDetailsController(IBookBorrowingRequestDetailsService bookBorrowingRequestDetailsService) 
        {
            _bookBorrowingRequestDetailsService = bookBorrowingRequestDetailsService;
        }

        [HttpGet("{borrowingRequestId}")]
        public async Task<ActionResult<IEnumerable<RequestDetailsToReturnDTO>>> GetRequestDetailsByBorrowingRequestId(int borrowingRequestId)
        {
            var requestDetails = await _bookBorrowingRequestDetailsService.GetRequestDetailsByBorrowingRequestId(borrowingRequestId);
            return Ok(requestDetails);
        }
    }
}
