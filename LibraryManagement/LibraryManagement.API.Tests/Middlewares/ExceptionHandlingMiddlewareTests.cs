using FluentValidation;
using LibraryManagement.API.Middlewares;
using LibraryManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagement.API.Tests.Middlewares
{
    [TestFixture]
    internal class ExceptionHandlingMiddlewareTests
    {
        private Mock<RequestDelegate> _mockNext;
        private Mock<ILogger<ExceptionHandlingMiddleware>> _mockLogger;
        private ExceptionHandlingMiddleware _middleware;
        private DefaultHttpContext _httpContext;

        [SetUp]
        public void Setup()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            _middleware = new ExceptionHandlingMiddleware(_mockNext.Object, _mockLogger.Object);
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
        }

        private async Task<string> GetResponseBody()
        {
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(_httpContext.Response.Body);
            return await reader.ReadToEndAsync();
        }

        [Test]
        public async Task InvokeAsync_WhenNoException_ShouldNotModifyResponse()
        {
            // Arrange
            _mockNext.Setup(x => x.Invoke(_httpContext)).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(200));
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Never);
        }

        [Test]
        public async Task HandleExceptionAsync_WithNotFoundException_ShouldReturn404()
        {
            // Arrange
            var exception = new NotFoundException("Book not found");

            // Act
            await _middleware.HandleExceptionAsync(_httpContext, exception);
            var responseBody = await GetResponseBody();

            // Assert
            Assert.Multiple(() => 
            {               
                Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
                Assert.That(_httpContext.Response.ContentType, Is.EqualTo("application/json; charset=utf-8"));
                Assert.That(responseBody, Contains.Substring("Resource not found"));
                Assert.That(responseBody, Contains.Substring("Book not found"));
            });
        }

        [Test]
        public async Task HandleExceptionAsync_WithValidationException_ShouldReturn400()
        {
            // Arrange
            var exception = new ValidationException("Validation failed");

            // Act
            await _middleware.HandleExceptionAsync(_httpContext, exception);
            var responseBody = await GetResponseBody();

            // Assert
            Assert.Multiple(() =>
            {               
                Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
                Assert.That(_httpContext.Response.ContentType, Is.EqualTo("application/json; charset=utf-8"));
                Assert.That(responseBody, Contains.Substring("Validation error"));
                Assert.That(responseBody, Contains.Substring("Validation failed"));
            });
            
        }

        [Test]
        public async Task HandleExceptionAsync_WithBadRequestException_ShouldReturn400()
        {
            // Arrange
            var exception = new BadRequestException("Invalid request");

            // Act
            await _middleware.HandleExceptionAsync(_httpContext, exception);
            var responseBody = await GetResponseBody();

            // Assert
            Assert.Multiple(() =>
            {               
                Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
                Assert.That(_httpContext.Response.ContentType, Is.EqualTo("application/json; charset=utf-8"));
                Assert.That(responseBody, Contains.Substring("Bad request"));
                Assert.That(responseBody, Contains.Substring("Invalid request"));
            });
            
        }

        [Test]
        public async Task HandleExceptionAsync_WithUnauthorizedException_ShouldReturn401()
        {
            // Arrange
            var exception = new UnauthorizedException("Access denied");

            // Act
            await _middleware.HandleExceptionAsync(_httpContext, exception);
            var responseBody = await GetResponseBody();

            // Assert
            Assert.Multiple(() =>
            {                
                Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));
                Assert.That(_httpContext.Response.ContentType, Is.EqualTo("application/json; charset=utf-8"));
                Assert.That(responseBody, Contains.Substring("Unauthorized"));
                Assert.That(responseBody, Contains.Substring("Access denied"));
            });
            
        }

        [Test]
        public async Task HandleExceptionAsync_WithGenericException_ShouldReturn500()
        {
            // Arrange
            var exception = new Exception("Something went wrong");

            // Act
            await _middleware.HandleExceptionAsync(_httpContext, exception);
            var responseBody = await GetResponseBody();

            // Assert
            Assert.Multiple(() =>
            {               
                Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
                Assert.That(_httpContext.Response.ContentType, Is.EqualTo("application/json; charset=utf-8"));
                Assert.That(responseBody, Contains.Substring("An unexpected error occured"));
                Assert.That(responseBody, Contains.Substring("Something went wrong"));
            });           
        }

        [Test]
        public async Task InvokeAsync_WhenExceptionThrown_ShouldHandleException()
        {
            // Arrange
            var exception = new NotFoundException("Not found");
            _mockNext.Setup(x => x.Invoke(_httpContext)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(_httpContext);
            var responseBody = await GetResponseBody();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
                Assert.That(_httpContext.Response.ContentType, Is.EqualTo("application/json; charset=utf-8"));
                Assert.That(responseBody, Contains.Substring("Not found"));
            });
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString()!.Contains("An exception occured")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
