using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Mappers;
using University.Application.Services;
using University.Domain.Entities;
using University.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Data;

namespace University.Tests.IntegrationTests.Services
{
    public class AccountServiceTests : IntegrationTestBase
    {
        private readonly AccountService _accountService;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<Users_Accounts> _passwordHasher;

        public AccountServiceTests() : base() // call base constructor to setup context
        {
            var accountRepository = new AccountRepository(context);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
            _passwordHasher = new PasswordHasher<Users_Accounts>();
            _accountService = new AccountService(accountRepository, _mapper, _passwordHasher);
        }

        [Fact]
        public async Task TestAdminAccountHasAdminRole()
        {
            // Arrange
            var expectedRoleId = SeedingConstants.AdminRoleId;
            var expectedAccountId = SeedingConstants.AdminAccountId;

            // Act
            var account = await context.Accounts
                .Include(a => a.roles)
                .ThenInclude(r => r.role)
                .FirstOrDefaultAsync(a => a.id == expectedAccountId);

            // Assert
            account.Should().NotBeNull();
            account.roles.Should().ContainSingle();
            account.roles[0].role_id.Should().Be(expectedRoleId);
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
                .Include(a => a.roles)
                .ThenInclude(r => r.role)
                .SingleOrDefaultAsync(a => a.id == newUser.Id);

            createdAccount.Should().NotBeNull("because the account should be created successfully.");
            createdAccount.roles.Should().ContainSingle();
            createdAccount.roles[0].role_id.Should().Be(SeedingConstants.StudentRoleId, "because the new user should be assigned the 'Student' role.");
            createdAccount.roles[0].role.name.Should().Be("Student", "because the correct role should be linked.");
        }

    }
}
