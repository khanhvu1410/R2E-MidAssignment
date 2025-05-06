using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Helpers;
using LibraryManagement.Infrastructure.Services;
using Moq;

namespace LibraryManagement.Infrastructure.Tests.Services
{
    [TestFixture]
    internal class AuthServiceTests
    {
        private Mock<IGenericRepository<User>> _userRepositoryMock;
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<IValidator<UserToLoginDTO>> _userToLoginDTOValidatorMock;
        private Mock<IValidator<UserToRegisterDTO>> _userToRegisterDTOValidatorMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IGenericRepository<User>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _userToLoginDTOValidatorMock = new Mock<IValidator<UserToLoginDTO>>();
            _userToRegisterDTOValidatorMock = new Mock<IValidator<UserToRegisterDTO>>();

            _authService = new AuthService(
                _userRepositoryMock.Object,
                _tokenServiceMock.Object,
                _userToLoginDTOValidatorMock.Object,
                _userToRegisterDTOValidatorMock.Object);
        }

        #region RegisterAsync Tests

        [Test]
        public async Task RegisterAsync_ValidUserToRegisterDTO_CreatesUserSuccessfully()
        {
            // Arrange
            var userToRegisterDTO = new UserToRegisterDTO
            {
                Username = "newuser",
                Password = "Password123!"
            };
            var validationResult = new ValidationResult(); // Valid result (no errors)
            _userToRegisterDTOValidatorMock
                .Setup(v => v.ValidateAsync(userToRegisterDTO, default))
                .ReturnsAsync(validationResult);

            _userRepositoryMock
                .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(false); // User does not exist

            var addedUser = new User { Username = userToRegisterDTO.Username }; // Mock the returned user
            _userRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(addedUser); // Return Task<User>

            // Act
            await _authService.RegisterAsync(userToRegisterDTO);

            // Assert
            _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(u =>
                u.Username == userToRegisterDTO.Username &&
                u.PasswordHash != null &&
                u.PasswordSalt != null)), Times.Once());
        }

        [Test]
        public void RegisterAsync_InvalidUserToRegisterDTO_ThrowsValidationException()
        {
            // Arrange
            var userToRegisterDTO = new UserToRegisterDTO
            {
                Username = "", // Invalid
                Password = "Password123!"
            };
            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("Username", "Username is required.")
            });
            _userToRegisterDTOValidatorMock
                .Setup(v => v.ValidateAsync(userToRegisterDTO, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(() => _authService.RegisterAsync(userToRegisterDTO));
            _userRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Never());
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public void RegisterAsync_UsernameAlreadyExists_ThrowsBadRequestException()
        {
            // Arrange
            var userToRegisterDTO = new UserToRegisterDTO
            {
                Username = "existinguser",
                Password = "Password123!"
            };
            var validationResult = new ValidationResult(); // Valid result
            _userToRegisterDTOValidatorMock
                .Setup(v => v.ValidateAsync(userToRegisterDTO, default))
                .ReturnsAsync(validationResult);

            _userRepositoryMock
                .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(true); // User exists

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() => _authService.RegisterAsync(userToRegisterDTO));
            Assert.That(ex.Message, Is.EqualTo("Username already exitsts."));
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never());
        }

        #endregion

        #region LoginAsync Tests

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var userToLoginDTO = new UserToLoginDTO
            {
                Username = "testuser",
                Password = "Password123!"
            };
            var validationResult = new ValidationResult(); // Valid result
            _userToLoginDTOValidatorMock
                .Setup(v => v.ValidateAsync(userToLoginDTO, default))
                .ReturnsAsync(validationResult);

            var user = new User
            {
                Id = 1,
                Username = userToLoginDTO.Username,
                PasswordHash = new byte[] { 1, 2, 3 },
                PasswordSalt = new byte[] { 4, 5, 6 },
                Role = UserRole.NormalUser
            };
            _userRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            // Mock PasswordHelper to verify password
            PasswordHelper.CreatePasswordHash(userToLoginDTO.Password, out var passwordHash, out var passwordSalt);
            _userRepositoryMock
                .Setup(r => r.GetAsync(u => u.Username == userToLoginDTO.Username))
                .ReturnsAsync(new User
                {
                    Id = 1,
                    Username = userToLoginDTO.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = UserRole.NormalUser
                });

            var token = "jwt_token";
            _tokenServiceMock
                .Setup(t => t.GenerateToken(It.IsAny<User>()))
                .Returns(token);

            // Act
            var result = await _authService.LoginAsync(userToLoginDTO);

            // Assert            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.User.Username, Is.EqualTo(userToLoginDTO.Username));
                Assert.That(result.AccessToken, Is.EqualTo(token));
            });
        }

        [Test]
        public void LoginAsync_InvalidUserToLoginDTO_ThrowsValidationException()
        {
            // Arrange
            var userToLoginDTO = new UserToLoginDTO
            {
                Username = "", // Invalid
                Password = "Password123!"
            };
            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("Username", "Username is required.")
            });
            _userToLoginDTOValidatorMock
                .Setup(v => v.ValidateAsync(userToLoginDTO, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(() => _authService.LoginAsync(userToLoginDTO));
            _userRepositoryMock.Verify(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Never());
            _tokenServiceMock.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public void LoginAsync_NonExistentUser_ThrowsUnauthorizedException()
        {
            // Arrange
            var userToLoginDTO = new UserToLoginDTO
            {
                Username = "nonexistent",
                Password = "Password123!"
            };
            var validationResult = new ValidationResult(); // Valid result
            _userToLoginDTOValidatorMock
                .Setup(v => v.ValidateAsync(userToLoginDTO, default))
                .ReturnsAsync(validationResult);

            _userRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User?)null); // User not found

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedException>(() => _authService.LoginAsync(userToLoginDTO));
            Assert.That(ex.Message, Is.EqualTo("Invalid username or password."));
            _tokenServiceMock.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public void LoginAsync_IncorrectPassword_ThrowsUnauthorizedException()
        {
            // Arrange
            var userToLoginDTO = new UserToLoginDTO
            {
                Username = "testuser",
                Password = "WrongPassword!"
            };
            var validationResult = new ValidationResult(); // Valid result
            _userToLoginDTOValidatorMock
                .Setup(v => v.ValidateAsync(userToLoginDTO, default))
                .ReturnsAsync(validationResult);

            var user = new User
            {
                Id = 1,
                Username = userToLoginDTO.Username,
                PasswordHash = new byte[] { 1, 2, 3 },
                PasswordSalt = new byte[] { 4, 5, 6 },
                Role = UserRole.NormalUser
            };
            _userRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            // Simulate PasswordHelper returning false for incorrect password
            PasswordHelper.CreatePasswordHash("CorrectPassword!", out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedException>(() => _authService.LoginAsync(userToLoginDTO));
            Assert.That(ex.Message, Is.EqualTo("Invalid username or password."));
            _tokenServiceMock.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never());
        }

        #endregion
    }
}
