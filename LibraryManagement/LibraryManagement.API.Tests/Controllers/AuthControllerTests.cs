using FluentValidation;
using LibraryManagement.API.Controllers;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.API.Tests.Controllers
{
    [TestFixture]
    internal class AuthControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _authController;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _authController = new AuthController(_mockAuthService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        #region Register Tests

        [Test]
        public async Task Register_WithValidUser_Returns201Created()
        {
            // Arrange
            var userToRegister = new UserToRegisterDTO
            {
                Username = "testuser",
                Password = "Test@123",
                Email = "test@example.com"
            };

            // Act
            var result = await _authController.Register(userToRegister);
            
            // Assert
            Assert.Multiple(() =>
            {    
                Assert.That(result, Is.InstanceOf<StatusCodeResult>());
                Assert.That((result as StatusCodeResult)?.StatusCode, Is.EqualTo(201));
            });
            _mockAuthService.Verify(x => x.RegisterAsync(userToRegister), Times.Once);
        }

        [Test]
        public void Register_WhenValidationFails_ThrowsValidationException()
        {
            // Arrange
            var userToRegister = new UserToRegisterDTO();
            var validationException = new ValidationException("Validation failed");

            _mockAuthService.Setup(x => x.RegisterAsync(userToRegister))
                .ThrowsAsync(validationException);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => _authController.Register(userToRegister));
            Assert.That(ex.Message, Is.EqualTo("Validation failed"));
        }

        [Test]
        public void Register_WhenUserExists_ThrowsBadRequestException()
        {
            // Arrange
            var userToRegister = new UserToRegisterDTO();
            var badRequestException = new BadRequestException("User already exists");

            _mockAuthService.Setup(x => x.RegisterAsync(userToRegister))
                .ThrowsAsync(badRequestException);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() =>
                _authController.Register(userToRegister));
            Assert.That(ex.Message, Is.EqualTo("User already exists"));
        }

        [Test]
        public void Register_WhenUnexpectedError_ThrowsException()
        {
            // Arrange
            var userToRegister = new UserToRegisterDTO();
            var exception = new Exception("Unexpected error");

            _mockAuthService.Setup(x => x.RegisterAsync(userToRegister))
                .ThrowsAsync(exception);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() =>
                _authController.Register(userToRegister));
            Assert.That(ex.Message, Is.EqualTo("Unexpected error"));
        }

        #endregion

        #region Login Tests

        [Test]
        public async Task Login_WithValidCredentials_ReturnsOkWithResponse()
        {
            // Arrange
            var userToLogin = new UserToLoginDTO
            {
                Username = "testuser",
                Password = "Test@123"
            };

            var expectedResponse = new LoginResponse
            {
                AccessToken = "test-token",
                User = new UserToReturnDTO { Username = "testuser" }
            };

            _mockAuthService.Setup(x => x.LoginAsync(userToLogin))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _authController.Login(userToLogin);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ActionResult<LoginResponse>>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult, Is.Not.Null);
                Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));
            });
            _mockAuthService.Verify(x => x.LoginAsync(userToLogin), Times.Once);
        }

        [Test]
        public void Login_WithInvalidCredentials_ThrowsUnauthorizedException()
        {
            // Arrange
            var userToLogin = new UserToLoginDTO
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            _mockAuthService.Setup(x => x.LoginAsync(userToLogin))
                .ThrowsAsync(new UnauthorizedException("Invalid credentials"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedException>(() =>
                _authController.Login(userToLogin));
            Assert.That(ex.Message, Is.EqualTo("Invalid credentials"));
        }

        [Test]
        public void Login_WhenUserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var userToLogin = new UserToLoginDTO
            {
                Username = "nonexistent",
                Password = "Test@123"
            };

            _mockAuthService.Setup(x => x.LoginAsync(userToLogin))
                .ThrowsAsync(new NotFoundException("User not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _authController.Login(userToLogin));
            Assert.That(ex.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public void Login_WhenValidationFails_ThrowsValidationException()
        {
            // Arrange
            var userToLogin = new UserToLoginDTO();
            var validationException = new ValidationException("Validation failed");

            _mockAuthService.Setup(x => x.LoginAsync(userToLogin))
                .ThrowsAsync(validationException);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() =>
                _authController.Login(userToLogin));
            Assert.That(ex.Message, Is.EqualTo("Validation failed"));
        }

        [Test]
        public void Login_WhenUnexpectedError_ThrowsException()
        {
            // Arrange
            var userToLogin = new UserToLoginDTO();
            var exception = new Exception("Unexpected error");

            _mockAuthService.Setup(x => x.LoginAsync(userToLogin))
                .ThrowsAsync(exception);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() =>
                _authController.Login(userToLogin));
            Assert.That(ex.Message, Is.EqualTo("Unexpected error"));
        }

        #endregion
    }
}
