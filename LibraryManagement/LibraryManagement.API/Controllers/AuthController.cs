using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDTO userToRegisterDTO)
        {
            await _authService.RegisterAsync(userToRegisterDTO);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToReturnDTO>> Login(UserToLoginDTO userToLoginDTO)
        {
            var user = await _authService.LoginAsync(userToLoginDTO);
            return Ok(user);
        }
    }
}
