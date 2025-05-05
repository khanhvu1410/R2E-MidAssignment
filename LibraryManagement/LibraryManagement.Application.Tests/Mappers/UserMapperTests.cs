using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.Tests.Mappers
{
    [TestFixture]
    internal class UserMapperTests
    {
        #region ToUserToReturnDTO Tests

        [Test]
        public void ToUserToReturnDTO_WithCompleteUser_ReturnsCorrectDTO()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                Role = UserRole.NormalUser,
                PasswordHash = new byte[] { 0x01, 0x02 },
                PasswordSalt = new byte[] { 0x03, 0x04 }
            };

            // Act
            var result = user.ToUserToReturnDTO();

            // Assert           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Username, Is.EqualTo(user.Username));
                Assert.That(result.Email, Is.EqualTo(user.Email));
                Assert.That(result.Role, Is.EqualTo(user.Role));

                // Verify sensitive data is not exposed
                Assert.That(result.GetType().GetProperty("PasswordHash"), Is.Null);
                Assert.That(result.GetType().GetProperty("PasswordSalt"), Is.Null);
            });
        }

        [Test]
        public void ToUserToReturnDTO_WithMinimumData_ReturnsCorrectDTO()
        {
            // Arrange
            var user = new User
            {
                Username = "minimaluser"
            };

            // Act
            var result = user.ToUserToReturnDTO();

            // Assert           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Username, Is.EqualTo("minimaluser"));
                Assert.That(result.Email, Is.Null);
                Assert.That(result.Role, Is.EqualTo(UserRole.NormalUser));
            });
        }

        [Test]
        public void ToUserToReturnDTO_WithNullInput_ThrowsArgumentNullException()
        {
            // Arrange
            User? user = null;

            // Act & Assert
            Assert.That(user?.ToUserToReturnDTO(), Is.Null);
        }

        #endregion

        #region ToUser Tests

        [Test]
        public void ToUser_WithCompleteRegistrationDTO_ReturnsCorrectUser()
        {
            // Arrange
            var registerDto = new UserToRegisterDTO
            {
                Username = "newuser",
                Email = "new@example.com",
                Role = UserRole.SuperUser,
            };
            var passwordHash = new byte[] { 0x05, 0x06 };
            var passwordSalt = new byte[] { 0x07, 0x08 };

            // Act
            var result = registerDto.ToUser(passwordHash, passwordSalt);

            // Assert     
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Username, Is.EqualTo(registerDto.Username));
                Assert.That(result.Email, Is.EqualTo(registerDto.Email));
                Assert.That(result.Role, Is.EqualTo(registerDto.Role));
                Assert.That(result.PasswordHash, Is.EqualTo(passwordHash));
                Assert.That(result.PasswordSalt, Is.EqualTo(passwordSalt));
            });
        }

        [Test]
        public void ToUser_WithMinimumRegistrationDTO_ReturnsCorrectUser()
        {
            // Arrange
            var registerDto = new UserToRegisterDTO
            {
                Username = "minimal"
            };
            var passwordHash = new byte[] { 0x01 };
            var passwordSalt = new byte[] { 0x02 };

            // Act
            var result = registerDto.ToUser(passwordHash, passwordSalt);

            // Assert           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Username, Is.EqualTo("minimal"));
                Assert.That(result.Email, Is.Null);
                Assert.That(result.Role, Is.EqualTo(UserRole.NormalUser));
                Assert.That(result.PasswordHash, Is.EqualTo(passwordHash));
                Assert.That(result.PasswordSalt, Is.EqualTo(passwordSalt));
            });
        }

        [Test]
        public void ToUser_WithNullRegistrationDTO_ReturnsNullUser()
        {
            // Arrange
            UserToRegisterDTO? registerDto = null;
            var passwordHash = new byte[] { 0x01 };
            var passwordSalt = new byte[] { 0x02 };

            // Act & Assert
            Assert.That(registerDto?.ToUser(passwordHash, passwordSalt), Is.Null);
        }

        [Test]
        public void ToUser_WithNullPasswordHash_ReturnsUserWithNullPasswordHash()
        {
            // Arrange
            var registerDto = new UserToRegisterDTO { Username = "test" };
            byte[]? passwordHash = null;
            var passwordSalt = new byte[] { 0x01 };

            // Act
            var result = registerDto.ToUser(passwordHash, passwordSalt);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.PasswordHash, Is.Null);
                Assert.That(result.PasswordSalt, Is.EqualTo(passwordSalt));
                Assert.That(result.Username, Is.EqualTo(registerDto.Username));
                Assert.That(result.Email, Is.EqualTo(registerDto.Email));
                Assert.That(result.Role, Is.EqualTo(registerDto.Role));
            });
        }

        [Test]
        public void ToUser_WithNullPasswordSalt_ReturnsUserWithNullPasswordSalt()
        {
            // Arrange
            var registerDto = new UserToRegisterDTO { Username = "test" };
            var passwordHash = new byte[] { 0x01 };
            byte[]? passwordSalt = null;

            // Act
            var result = registerDto.ToUser(passwordHash, passwordSalt);

            // Assert
            Assert.Multiple(() =>
            {             
                Assert.That(result.PasswordHash, Is.EqualTo(passwordHash));
                Assert.That(result.PasswordSalt, Is.Null);
                Assert.That(result.Username, Is.EqualTo(registerDto.Username));
                Assert.That(result.Email, Is.EqualTo(registerDto.Email));
                Assert.That(result.Role, Is.EqualTo(registerDto.Role));
            });
        }

        #endregion

        #region Security Tests

        [Test]
        public void ToUserToReturnDTO_DoesNotExposeSensitiveData()
        {
            // Arrange
            var user = new User
            {
                Username = "secureuser",
                PasswordHash = new byte[] { 0x7A, 0x12 },
                PasswordSalt = new byte[] { 0x7A, 0x22 }
            };

            // Act
            var result = user.ToUserToReturnDTO();

            // Assert
            var dtoProperties = result.GetType().GetProperties();
            Assert.Multiple(() =>
            {
                Assert.That(dtoProperties.Any(p => p.Name == "PasswordHash"), Is.False);
                Assert.That(dtoProperties.Any(p => p.Name == "PasswordSalt"), Is.False);
            });
        }

        #endregion
    }
}
