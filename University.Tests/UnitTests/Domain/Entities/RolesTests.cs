using System;
using FluentAssertions;
using University.Domain.Entities;
using Xunit;

namespace University.Tests.UnitTests.Domain.Entities
{
    public class RolesTests
    {
        [Fact]
        public void Constructor_InitializesRolesWithValidData()
        {
            // Arrange
            var roleName = "Admin";

            // Act
            var role = new Roles
            {
                Id = Guid.NewGuid(),
                Name = roleName
            };

            // Assert
            role.Name.Should().Be(roleName);
        }
    }
}
