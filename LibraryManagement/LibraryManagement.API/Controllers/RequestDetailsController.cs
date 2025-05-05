using LibraryManagement.Application.DTOs.RequestDetails;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RequestDetailsController : ControllerBase
    {
        private readonly IRequestDetailsService _requestDetailsService;

        public RequestDetailsController(IRequestDetailsService requestDetailsService) 
        {
            _requestDetailsService = requestDetailsService;
        }

        [HttpGet("{borrowingRequestId}")]
        public async Task<ActionResult<IEnumerable<RequestDetailsToReturnDTO>>> GetRequestDetailsByBorrowingRequestId(int borrowingRequestId)
        {
            var requestDetails = await _requestDetailsService.GetRequestDetailsByBorrowingRequestId(borrowingRequestId);
            return Ok(requestDetails);
        }
    }
}
