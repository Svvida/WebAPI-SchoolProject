using AutoMapper;
using Moq;
using Xunit;
using University.Application.DTOs;
using University.Application.Services;
using University.Domain.Entities;
using University.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Application.Mappers;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace University.Tests.UnitTests.Application.Services
{
    public class AccountServiceTests 
    {
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly IMapper _mapper; // Assuming AutoMapper configuration is already done in your setup.
        private readonly AccountService _service;
        private readonly PasswordHasher<Users_Accounts> _passwordHasher;

        public AccountServiceTests()
        {
            _mockRepo = new Mock<IAccountRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
            _service = new AccountService(_mockRepo.Object, _mapper, _passwordHasher);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsAllAccounts()
        {
            var accounts = new List<Users_Accounts>
        {
            new Users_Accounts { id = Guid.NewGuid(), email = "test1@example.com", login = "test1" },
            new Users_Accounts { id = Guid.NewGuid(), email = "test2@example.com", login = "test2" }
        };
            _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(accounts);

            var result = await _service.GetAllAccountsAsync();

            Assert.Equal(accounts.Count, result.Count());
        }

        [Fact]
        public async Task GetAccountByIdAsync_ReturnsAccount_IfExists()
        {
            var accountId = Guid.NewGuid();
            var account = new Users_Accounts { id = accountId, email = "test@example.com", login = "test" };
            _mockRepo.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(account);

            var result = await _service.GetAccountByIdAsync(accountId);

            Assert.NotNull(result);
            Assert.Equal(account.email, result.Email);
        }

        [Fact]
        public async Task AddAccountAsync_AddsAccount()
        {
            var accountDto = new AccountDto { Email = "new@example.com", Login = "newlogin" };
            var account = _mapper.Map<Users_Accounts>(accountDto);
            _mockRepo.Setup(x => x.AddAsync(It.IsAny<Users_Accounts>())).ReturnsAsync(account);

            await _service.AddAccountAsync(accountDto);

            _mockRepo.Verify(x => x.AddAsync(It.IsAny<Users_Accounts>()), Times.Once());
        }

    }
}
