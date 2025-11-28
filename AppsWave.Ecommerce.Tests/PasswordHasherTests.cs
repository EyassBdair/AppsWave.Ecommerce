using AppsWave.Ecommerce.Infrastructure.Services;
using Xunit;

namespace AppsWave.Ecommerce.Tests
{
    public class PasswordHasherTests
    {
        [Fact]
        public void HashPassword_ShouldReturnDifferentHash_ForSamePassword()
        {
            // Arrange
            var password = "TestPassword123";

            // Act
            var hash1 = PasswordHasher.HashPassword(password);
            var hash2 = PasswordHasher.HashPassword(password);

            // Assert
            Assert.NotEqual(hash1, hash2); // BCrypt generates different hashes each time
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordMatches()
        {
            // Arrange
            var password = "TestPassword123";
            var hash = PasswordHasher.HashPassword(password);

            // Act
            var result = PasswordHasher.VerifyPassword(password, hash);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatch()
        {
            // Arrange
            var correctPassword = "TestPassword123";
            var wrongPassword = "WrongPassword";
            var hash = PasswordHasher.HashPassword(correctPassword);

            // Act
            var result = PasswordHasher.VerifyPassword(wrongPassword, hash);

            // Assert
            Assert.False(result);
        }
    }
}

