using LibraryManagement.Infrastructure.Helpers;

namespace LibraryManagement.Infrastructure.Tests.Helpers
{
    [TestFixture]
    internal class PasswordHelperTests
    {
        #region CreatePasswordHash Tests

        [Test]
        public void CreatePasswordHash_WithValidPassword_GeneratesHashAndSalt()
        {
            // Arrange
            const string password = "TestPassword123!";

            // Act
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(passwordHash, Is.Not.Null);
                Assert.That(passwordHash, Has.Length.EqualTo(64)); // SHA512 produces 64-byte hashes
                Assert.That(passwordSalt, Is.Not.Null);
                Assert.That(passwordSalt, Has.Length.EqualTo(128)); // HMACSHA512 key is 128 bytes
            });
        }

        [Test]
        public void CreatePasswordHash_WithNullPassword_ThrowsArgumentNullException()
        {
            // Arrange
            const string? nullPassword = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                PasswordHelper.CreatePasswordHash(nullPassword ?? default!, out byte[] passwordHash, out byte[] passwordSalt));
        }

        [Test]
        public void CreatePasswordHash_WithSamePassword_ProducesDifferentSalts()
        {
            // Arrange
            const string password = "TestPassword123!";

            // Act
            PasswordHelper.CreatePasswordHash(password, out byte[] hash1, out byte[] salt1);
            PasswordHelper.CreatePasswordHash(password, out byte[] hash2, out byte[] salt2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(salt1, Is.Not.EqualTo(salt2), "Salts should be different");
                Assert.That(hash1, Is.Not.EqualTo(hash2), "Hashes should be different");
            });
        }

        #endregion

        #region VerifyPasswordHash Tests

        [Test]
        public void VerifyPasswordHash_WithCorrectPassword_ReturnsTrue()
        {
            // Arrange
            const string password = "TestPassword123!";
            PasswordHelper.CreatePasswordHash(password, out byte[] storedHash, out byte[] storedSalt);

            // Act
            var result = PasswordHelper.VerifyPasswordHash(password, storedHash, storedSalt);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void VerifyPasswordHash_WithIncorrectPassword_ReturnsFalse()
        {
            // Arrange
            const string originalPassword = "TestPassword123!";
            const string wrongPassword = "WrongPassword123!";
            PasswordHelper.CreatePasswordHash(originalPassword, out byte[] storedHash, out byte[] storedSalt);

            // Act
            var result = PasswordHelper.VerifyPasswordHash(wrongPassword, storedHash, storedSalt);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void VerifyPasswordHash_WithTamperedHash_ReturnsFalse()
        {
            // Arrange
            const string password = "TestPassword123!";
            PasswordHelper.CreatePasswordHash(password, out byte[] storedHash, out byte[] storedSalt);

            // Tamper with the hash
            storedHash[0] ^= 0xFF; // Flip all bits in the first byte

            // Act
            var result = PasswordHelper.VerifyPasswordHash(password, storedHash, storedSalt);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void VerifyPasswordHash_WithTamperedSalt_ReturnsFalse()
        {
            // Arrange
            const string password = "TestPassword123!";
            byte[] storedSalt;
            PasswordHelper.CreatePasswordHash(password, out byte[] storedHash, out storedSalt);

            // Tamper with the salt
            storedSalt[0] ^= 0xFF; // Flip all bits in the first byte

            // Act
            var result = PasswordHelper.VerifyPasswordHash(password, storedHash, storedSalt);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void VerifyPasswordHash_WithNullHash_ThrowsArgumentNullException()
        {
            // Arrange
            const string password = "TestPassword123!";
            byte[] salt = new byte[128]; // Valid salt size
            byte[]? hash = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                PasswordHelper.VerifyPasswordHash(password, hash ?? default!, salt));
        }

        [Test]
        public void VerifyPasswordHash_WithNullSalt_ThrowsArgumentNullException()
        {
            // Arrange
            const string password = "TestPassword123!";
            byte[] hash = new byte[64]; // Valid hash size
            byte[]? salt = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                PasswordHelper.VerifyPasswordHash(password, hash, salt ?? default!));
        }

        [Test]
        public void VerifyPasswordHash_WithEmptyPassword_ReturnsFalse()
        {
            // Arrange
            const string originalPassword = "TestPassword123!";
            const string emptyPassword = "";
            PasswordHelper.CreatePasswordHash(originalPassword, out byte[] storedHash, out byte[] storedSalt);

            // Act
            var result = PasswordHelper.VerifyPasswordHash(emptyPassword, storedHash, storedSalt);

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion

        #region Integration Tests

        [Test]
        public void CreateAndVerifyPasswordHash_WithValidPassword_WorksCorrectly()
        {
            // Arrange
            const string password = "ComplexPassword!@#123";

            // Act - Create hash
            PasswordHelper.CreatePasswordHash(password, out byte[] hash, out byte[] salt);

            // Assert - Verify
            Assert.That(PasswordHelper.VerifyPasswordHash(password, hash, salt), Is.True);
            Assert.That(PasswordHelper.VerifyPasswordHash(password + "x", hash, salt), Is.False);
        }

        [Test]
        public void VerifyPasswordHash_WithDifferentCases_ReturnsFalse()
        {
            // Arrange
            const string originalPassword = "TestPassword123!";
            const string caseChangedPassword = "testpassword123!";
            PasswordHelper.CreatePasswordHash(originalPassword, out byte[] storedHash, out byte[] storedSalt);

            // Act
            var result = PasswordHelper.VerifyPasswordHash(caseChangedPassword, storedHash, storedSalt);

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion
    }
}
