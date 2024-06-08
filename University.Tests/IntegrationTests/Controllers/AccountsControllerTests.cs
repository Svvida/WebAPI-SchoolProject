using Moq;
using Xunit;
using University.Application.DTOs;
using University.Application.Services;
using Microsoft.AspNetCore.Mvc;
using University.RestApi.Controllers;
using FluentAssertions;
using University.Application.Interfaces;

namespace University.Tests.IntegrationTests.Controllers
{
    public class AccountsControllerTests : IntegrationTestBase
    {
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly AccountsController _controller;

        public AccountsControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AccountsController(_mockAccountService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfAccounts()
        {
            // Arrange
            var accounts = new List<AccountDto>
            {
                new AccountDto { Id = Guid.NewGuid(), Email = "admin@wsei.pl", Login = "admin", IsActive = true },
                new AccountDto { Id = Guid.NewGuid(), Email = "user@wsei.pl", Login = "user", IsActive = true }
            };
            _mockAccountService.Setup(service => service.GetAllAccountsAsync()).ReturnsAsync(accounts);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(accounts);
        }

        [Fact]
        public async Task Get_ReturnsAccount_WhenAccountExists()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var account = new AccountDto { Id = accountId, Email = "admin@wsei.pl", Login = "admin", IsActive = true };
            _mockAccountService.Setup(service => service.GetAccountByIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _controller.Get(accountId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(account);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAccount()
        {
            // Arrange
            var newAccount = new AccountDto { Email = "new@wsei.pl", Login = "newuser", Password = "newpassword", IsActive = true };
            _mockAccountService.Setup(service => service.AddAccountAsync(newAccount)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(newAccount);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.Value.Should().BeEquivalentTo(newAccount);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenAccountIsUpdated()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var updatedAccount = new AccountDto { Id = accountId, Email = "updated@wsei.pl", Login = "updateduser", IsActive = true };
            var existingAccount = new AccountDto { Id = accountId, Email = "existing@wsei.pl", Login = "existinguser", IsActive = true };

            _mockAccountService.Setup(service => service.GetAccountByIdAsync(accountId)).ReturnsAsync(existingAccount);
            _mockAccountService.Setup(service => service.UpdateAccountAsync(updatedAccount)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(accountId, updatedAccount);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenAccountIsDeleted()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var existingAccount = new AccountDto { Id = accountId, Email = "delete@wsei.pl", Login = "deleteuser", IsActive = true };

            _mockAccountService.Setup(service => service.GetAccountByIdAsync(accountId)).ReturnsAsync(existingAccount);
            _mockAccountService.Setup(service => service.DeleteAccountAsync(accountId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(accountId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
