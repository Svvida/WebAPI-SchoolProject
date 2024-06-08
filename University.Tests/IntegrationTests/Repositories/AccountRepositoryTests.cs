using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Domain.Entities;
using University.Infrastructure.Data;
using University.Infrastructure.Repositories;
using Xunit;

namespace University.Tests.IntegrationTests.Repositories
{
    public class AccountRepositoryTests : IntegrationTestBase
    {
        private readonly AccountRepository _repository;

        public AccountRepositoryTests()
        {
            _repository = new AccountRepository(context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAccounts()
        {
            var accounts = await _repository.GetAllAsync();
            Assert.NotEmpty(accounts);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAccount_IfExists()
        {
            var accountId = SeedingConstants.AdminAccountId; // Use seeded account ID
            var account = await _repository.GetByIdAsync(accountId);
            Assert.NotNull(account);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_IfNotExists()
        {
            var accountId = Guid.NewGuid();
            var account = await _repository.GetByIdAsync(accountId);
            Assert.Null(account);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAccount()
        {
            var account = new Users_Accounts
            {
                Id = Guid.NewGuid(),
                Email = "newuser@example.com",
                Login = "newuser",
                Password = "password",
                IsActive = true
            };

            await _repository.AddAsync(account);
            var accounts = await _repository.GetAllAsync();
            Assert.Contains(accounts, a => a.Email == "newuser@example.com");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAccount()
        {
            var accountId = SeedingConstants.AdminAccountId; // Use seeded account ID
            var account = await _repository.GetByIdAsync(accountId);
            account.Email = "updated@example.com";

            await _repository.UpdateAsync(account);
            var updatedAccount = await _repository.GetByIdAsync(accountId);

            Assert.Equal("updated@example.com", updatedAccount.Email);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAccount()
        {
            var accountId = SeedingConstants.AdminAccountId; // Use seeded account ID
            await _repository.DeleteAsync(accountId);

            var deletedAccount = await _repository.GetByIdAsync(accountId);
            Assert.Null(deletedAccount);
        }

        [Fact]
        public async Task AddRoleToAccountAsync_ShouldAddRole()
        {
            var accountId = SeedingConstants.AdminAccountId; // Use seeded account ID
            var roleId = SeedingConstants.StudentRoleId; // Use seeded role ID

            await _repository.AddRoleToAccountAsync(accountId, roleId);
            var account = await _repository.GetByIdAsync(accountId);

            Assert.Contains(account.Roles, r => r.RoleId == roleId);
        }

        [Fact]
        public async Task DeleteRoleFromAccountAsync_ShouldDeleteRole()
        {
            var accountId = SeedingConstants.AdminAccountId; // Use seeded account ID
            var roleId = SeedingConstants.AdminRoleId; // Use seeded role ID

            await _repository.DeleteRoleFromAccountAsync(accountId, roleId);
            var account = await _repository.GetByIdAsync(accountId);

            Assert.DoesNotContain(account.Roles, r => r.RoleId == roleId);
        }

        [Fact]
        public async Task GetByLoginAsync_ShouldReturnAccount_IfExists()
        {
            var login = "admin"; // Use seeded login
            var account = await _repository.GetByLoginAsync(login);
            Assert.NotNull(account);
        }

        [Fact]
        public async Task GetByLoginAsync_ShouldReturnNull_IfNotExists()
        {
            var login = "nonexistentlogin";
            var account = await _repository.GetByLoginAsync(login);
            Assert.Null(account);
        }
    }
}
