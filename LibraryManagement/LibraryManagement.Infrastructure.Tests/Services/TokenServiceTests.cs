using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrastructure.Services;
using LibraryManagement.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagement.Infrastructure.Tests.Services
{
    [TestFixture]
    internal class TokenServiceTests
    {
        private Mock<IOptions<JwtSettings>> _mockJwtSettings;
        private TokenService _tokenService;
        private JwtSettings _jwtSettings;

        [SetUp]
        public void Setup()
        {
            _jwtSettings = new JwtSettings
            {
                Secret = "ThisIsAVeryLongSecretKeyThatNeedsToBeAtLeast512BitsLongForHMACSHA512Algorithm1234567890",
                ExpiryMinutes = 60,
                Issuer = "LibraryManagement",
                Audience = "LibraryClients"
            };

            _mockJwtSettings = new Mock<IOptions<JwtSettings>>();
            _mockJwtSettings.Setup(x => x.Value).Returns(_jwtSettings);

            _tokenService = new TokenService(_mockJwtSettings.Object);
        }

        [Test]
        public void GenerateToken_WithValidUser_ReturnsValidJwtToken()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Role = UserRole.NormalUser
            };

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            Assert.That(token, Is.Not.Null.And.Not.Empty);

            // Validate token structure
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret))
            };

            // This will throw if the token is invalid
            _ = tokenHandler.ValidateToken(token, validationParameters, out _);
        }

        [Test]
        public void GenerateToken_WithDifferentUsers_ProducesDifferentTokens()
        {
            // Arrange
            var user1 = new User { Id = 1, Username = "user1", Role = UserRole.NormalUser };
            var user2 = new User { Id = 2, Username = "user2", Role = UserRole.SuperUser };

            // Act
            var token1 = _tokenService.GenerateToken(user1);
            var token2 = _tokenService.GenerateToken(user2);

            // Assert
            Assert.That(token1, Is.Not.EqualTo(token2));
        }

        [Test]
        public void GenerateToken_WithValidUser_HasCorrectExpiration()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", Role = UserRole.NormalUser };
            var expectedExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            // Act
            var token = _tokenService.GenerateToken(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Assert
            // Allow for small time difference due to test execution time
            Assert.That(jwtToken.ValidTo,
                Is.EqualTo(expectedExpiration).Within(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void GenerateToken_WithEmptyUsername_StillGeneratesToken()
        {
            // Arrange
            var user = new User { Id = 1, Username = "", Role = UserRole.NormalUser };

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            Assert.That(token, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void GenerateToken_ContainsCorrectIssuerAndAudience()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", Role = UserRole.NormalUser };

            // Act
            var token = _tokenService.GenerateToken(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(jwtToken.Issuer, Is.EqualTo(_jwtSettings.Issuer));
                Assert.That(jwtToken.Audiences.First(), Is.EqualTo(_jwtSettings.Audience));
            });
        }
    }
}
