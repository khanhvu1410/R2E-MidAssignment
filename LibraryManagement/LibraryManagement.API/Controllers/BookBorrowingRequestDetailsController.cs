using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookBorrowingRequestDetailsController : ControllerBase
    {
        private readonly IBookBorrowingRequestDetailsService _bookBorrowingRequestDetailsService;

        public BookBorrowingRequestDetailsController(IBookBorrowingRequestDetailsService bookBorrowingRequestDetailsService) 
        {
            _bookBorrowingRequestDetailsService = bookBorrowingRequestDetailsService;
        }

        [HttpGet("{borrowingRequetsId}")]
        public async Task<ActionResult<IEnumerable<RequestDetailsToReturnDTO>>> GetRequestDetailsByBorrowingRequestId(int borrowingRequetsId)
        {
            var requestDetails = await _bookBorrowingRequestDetailsService.GetRequestDetailsByBorrowingRequestId(borrowingRequetsId);
            return Ok(requestDetails);
        }
    }
}
