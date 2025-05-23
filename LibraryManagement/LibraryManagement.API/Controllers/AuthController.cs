﻿using LibraryManagement.Application.Common;
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

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserToRegisterDTO userToRegisterDTO)
        {
            await _authService.RegisterAsync(userToRegisterDTO);
            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(UserToLoginDTO userToLoginDTO)
        {
            var response = await _authService.LoginAsync(userToLoginDTO);
            return Ok(response);
        }
    }
}
