using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;
using University.Application.Services;
using University.Domain.Entities;
using University.Domain.Interfaces;
using Xunit;

namespace University.Tests.UnitTests.Application.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IPasswordHasher<Users_Accounts?>> _mockPasswordHasher;
        private readonly AccountService _accountService;

        public AuthServiceTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockPasswordHasher = new Mock<IPasswordHasher<Users_Accounts?>>();
            _accountService = new AccountService(_mockAccountRepository.Object, _mockMapper.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsAccounts()
        {
            // Arrange
            var accounts = new List<Users_Accounts>
            {
                new Users_Accounts { Id = Guid.NewGuid(), Email = "test1@example.com", Login = "test1" },
                new Users_Accounts { Id = Guid.NewGuid(), Email = "test2@example.com", Login = "test2" }
            };
            _mockAccountRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(accounts);

            // Act
            var result = await _accountService.GetAllAccountsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        // Add more tests for other methods
    }
}
