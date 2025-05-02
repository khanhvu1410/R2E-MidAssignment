using FluentValidation;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Helpers;

namespace LibraryManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IValidator<UserToLoginDTO> _userToLoginDTOValidator;
        private readonly IValidator<UserToRegisterDTO> _userToRegisterDTOValidator;

        public AuthService(IGenericRepository<User> userRepository, 
            ITokenService tokenService,
            IValidator<UserToLoginDTO> userToLoginDTOValidator,
            IValidator<UserToRegisterDTO> userToRegisterDTOValidator)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _userToLoginDTOValidator = userToLoginDTOValidator;
            _userToRegisterDTOValidator = userToRegisterDTOValidator;
        }

        public async Task RegisterAsync(UserToRegisterDTO userToRegisterDTO)
        {
            var result = await _userToRegisterDTOValidator.ValidateAsync(userToRegisterDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            // Check if user already exist
            var userExists = await _userRepository.ExistsAsync(u => u.Username == userToRegisterDTO.Username);
            if (userExists)
            {
                throw new BadRequestException("Username already exitsts.");
            }

            PasswordHelper.CreatePasswordHash(userToRegisterDTO.Password, out var passwordHash, out var passwordSalt);
            var user = userToRegisterDTO.ToUser(passwordHash, passwordSalt);
            await _userRepository.AddAsync(user);
        }

        public async Task<LoginResponse> LoginAsync(UserToLoginDTO userToLoginDTO)
        {
            var result = await _userToLoginDTOValidator.ValidateAsync(userToLoginDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var user = await _userRepository.GetAsync(u => u.Username == userToLoginDTO.Username);
            if (user == null || !PasswordHelper.VerifyPasswordHash(userToLoginDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedException($"Invalid username or password.");
            }

            var accessToken = _tokenService.GenerateToken(user);
            return new LoginResponse
            {
                User = user.ToUserToReturnDTO(),
                AccessToken = accessToken
            };
        }
    }
}
