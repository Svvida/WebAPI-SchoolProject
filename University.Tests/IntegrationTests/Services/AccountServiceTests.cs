using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Mappers;
using University.Application.Services;
using University.Domain.Entities;
using University.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Data;
using Xunit;

namespace University.Tests.IntegrationTests.Services
{
    public class AccountServiceTests : IntegrationTestBase
    {
        private readonly AccountService _accountService;
        private readonly IPasswordHasher<Users_Accounts> _passwordHasher;

        public AccountServiceTests() : base() // call base constructor to setup context
        {
            var accountRepository = new AccountRepository(context);
            _passwordHasher = new PasswordHasher<Users_Accounts>();
            _accountService = new AccountService(accountRepository, mapper, _passwordHasher);
        }

        [Fact]
        public async Task TestAdminAccountHasAdminRole()
        {
            // Arrange
            var expectedRoleId = SeedingConstants.AdminRoleId;
            var expectedAccountId = SeedingConstants.AdminAccountId;

            // Act
            var account = await context.Accounts
                .Include(a => a.Roles)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(a => a.Id == expectedAccountId);

            // Assert
            account.Should().NotBeNull();
            account.Roles.Should().ContainSingle();
            account.Roles[0].RoleId.Should().Be(expectedRoleId);
        }

        [Fact]
        public async Task CreateUserAndAssignRole_ShouldAssignCorrectRole()
        {
            // Arrange
            var newUser = new AccountDto
            {
                Id = Guid.NewGuid(),
                Email = "newuser@example.com",
                Login = "newuser",
                Password = "SecurePassword123",
                IsActive = true,
                Roles = new List<RoleDto>
                {
                    new RoleDto { Id = SeedingConstants.StudentRoleId, Name = "Student" }
                }
            };

            // Act
            await _accountService.AddAccountAsync(newUser);

            // Assert
            var createdAccount = await context.Accounts
                .Include(a => a.Roles)
                .ThenInclude(r => r.Role)
                .SingleOrDefaultAsync(a => a.Id == newUser.Id);

            createdAccount.Should().NotBeNull("because the account should be created successfully.");
            createdAccount.Roles.Should().ContainSingle();
            createdAccount.Roles[0].RoleId.Should().Be(SeedingConstants.StudentRoleId, "because the new user should be assigned the 'Student' role.");
            createdAccount.Roles[0].Role.Name.Should().Be("Student", "because the correct role should be linked.");
        }
    }
}
