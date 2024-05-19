using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Mappers;
using University.Application.Services;
using University.Domain.Entities;
using University.Domain.Interfaces;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace University.Tests.UnitTests.Application.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly AccountService _service;
        private readonly IPasswordHasher<Users_Accounts?> _passwordHasher;

        public AccountServiceTests()
        {
            _mockRepo = new Mock<IAccountRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
            _passwordHasher = new PasswordHasher<Users_Accounts?>();
            _service = new AccountService(_mockRepo.Object, _mapper, _passwordHasher);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsAllAccounts()
        {
            var accounts = new List<Users_Accounts>
            {
                new Users_Accounts { Id = Guid.NewGuid(), Email = "test1@example.com", Login = "test1" },
                new Users_Accounts { Id = Guid.NewGuid(), Email = "test2@example.com", Login = "test2" }
            };
            _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(accounts);

            var result = await _service.GetAllAccountsAsync();

            Assert.Equal(accounts.Count, result.Count());
        }

        [Fact]
        public async Task GetAccountByIdAsync_ReturnsAccount_IfExists()
        {
            var accountId = Guid.NewGuid();
            var account = new Users_Accounts { Id = accountId, Email = "test@example.com", Login = "test" };
            _mockRepo.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(account);

            var result = await _service.GetAccountByIdAsync(accountId);

            Assert.NotNull(result);
            Assert.Equal(account.Email, result.Email);
        }

        [Fact]
        public async Task AddAccountAsync_AddsAccount()
        {
            var accountDto = new AccountDto { Email = "new@example.com", Login = "newlogin", Password = "password" };
            var account = _mapper.Map<Users_Accounts>(accountDto);
            _mockRepo.Setup(x => x.AddAsync(It.IsAny<Users_Accounts>())).ReturnsAsync(account);

            await _service.AddAccountAsync(accountDto);

            _mockRepo.Verify(x => x.AddAsync(It.IsAny<Users_Accounts>()), Times.Once());
        }

        [Fact]
        public async Task UpdateAccountAsync_UpdatesAccount()
        {
            var accountDto = new AccountDto { Id = Guid.NewGuid(), Email = "updated@example.com", Login = "updatedlogin" };
            var account = _mapper.Map<Users_Accounts>(accountDto);
            _mockRepo.Setup(x => x.GetByIdAsync(accountDto.Id)).ReturnsAsync(account);

            await _service.UpdateAccountAsync(accountDto);

            _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<Users_Accounts>()), Times.Once());
        }

        [Fact]
        public async Task DeleteAccountAsync_DeletesAccount()
        {
            var accountId = Guid.NewGuid();
            var account = new Users_Accounts { Id = accountId, Email = "test@example.com", Login = "test" };
            _mockRepo.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(account);

            await _service.DeleteAccountAsync(accountId);

            _mockRepo.Verify(x => x.DeleteAsync(accountId), Times.Once());
        }
    }
}
