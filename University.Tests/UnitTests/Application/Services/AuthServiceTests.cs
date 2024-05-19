using Moq;
using System.Threading.Tasks;
using University.Domain.Entities;
using University.Domain.Interfaces;
using University.Application.Services;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace University.Tests.UnitTests.Application.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly AuthService _authService;
        private readonly IPasswordHasher<Users_Accounts> _passwordHasher;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IAccountRepository>();
            _passwordHasher = new PasswordHasher<Users_Accounts>();
            _authService = new AuthService(_mockRepo.Object, _passwordHasher);
        }

        [Fact]
        public async Task ValidateUserAsync_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var user = new Users_Accounts { Login = "testuser", Password = _passwordHasher.HashPassword(null, "password") };
            _mockRepo.Setup(repo => repo.GetByLoginAsync("testuser")).ReturnsAsync(user);

            // Act
            var result = await _authService.ValidateUserAsync("testuser", "password");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Login);
        }

        [Fact]
        public async Task ValidateUserAsync_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var user = new Users_Accounts { Login = "testuser", Password = _passwordHasher.HashPassword(null, "password") };
            _mockRepo.Setup(repo => repo.GetByLoginAsync("testuser")).ReturnsAsync(user);

            // Act
            var result = await _authService.ValidateUserAsync("testuser", "wrongpassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateUserAsync_NonExistentUser_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByLoginAsync("nonexistent")).ReturnsAsync((Users_Accounts)null);

            // Act
            var result = await _authService.ValidateUserAsync("nonexistent", "password");

            // Assert
            Assert.Null(result);
        }
    }
}
