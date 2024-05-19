using System;
using FluentAssertions;
using University.Domain.Entities;
using Xunit;

namespace University.Tests.UnitTests.Domain.Entities
{
    public class Users_AccountsTests
    {
        [Fact]
        public void Activate_ActivatesAccount()
        {
            // Arrange
            var userAccount = new Users_Accounts
            {
                Id = Guid.NewGuid(),
                Email = "user@example.com",
                Login = "user",
                Password = "password123",
                IsActive = false,
                DeactivationDate = DateTime.UtcNow
            };

            // Act
            userAccount.Activate();

            // Assert
            userAccount.IsActive.Should().BeTrue();
            userAccount.DeactivationDate.Should().BeNull();
        }

        [Fact]
        public void Deactivate_DeactivatesAccount()
        {
            // Arrange
            var userAccount = new Users_Accounts
            {
                Id = Guid.NewGuid(),
                Email = "user@example.com",
                Login = "user",
                Password = "password123",
                IsActive = true
            };

            // Act
            userAccount.Deactivate();

            // Assert
            userAccount.IsActive.Should().BeFalse();
            userAccount.DeactivationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}
