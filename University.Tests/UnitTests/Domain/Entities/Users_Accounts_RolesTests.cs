using System;
using FluentAssertions;
using University.Domain.Entities;
using Xunit;

namespace University.Tests.UnitTests.Domain.Entities
{
    public class Users_Accounts_RolesTests
    {
        [Fact]
        public void Constructor_InitializesUsersAccountsRolesWithValidData()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            // Act
            var usersAccountsRoles = new Users_Accounts_Roles
            {
                AccountId = accountId,
                RoleId = roleId
            };

            // Assert
            usersAccountsRoles.AccountId.Should().Be(accountId);
            usersAccountsRoles.RoleId.Should().Be(roleId);
        }
    }
}
