using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Domain.Entities;
using University.Infrastructure.Data;
using University.Infrastructure.Repositories;
using Xunit;

namespace University.Tests.IntegrationTests.Repositories
{
    public class RoleRepositoryTests : IntegrationTestBase
    {
        private readonly RoleRepository _repository;

        public RoleRepositoryTests()
        {
            _repository = new RoleRepository(context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRoles()
        {
            var roles = await _repository.GetAllAsync();
            Assert.NotEmpty(roles);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRole_IfExists()
        {
            var roleId = SeedingConstants.AdminRoleId; // Use seeded role ID
            var role = await _repository.GetByIdAsync(roleId);
            Assert.NotNull(role);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_IfNotExists()
        {
            var roleId = Guid.NewGuid();
            var role = await _repository.GetByIdAsync(roleId);
            Assert.Null(role);
        }

        [Fact]
        public async Task AddAsync_ShouldAddRole()
        {
            var role = new Roles
            {
                Id = Guid.NewGuid(),
                Name = "NewRole"
            };

            await _repository.AddAsync(role);
            var roles = await _repository.GetAllAsync();
            Assert.Contains(roles, r => r.Name == "NewRole");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateRole()
        {
            var roleId = SeedingConstants.AdminRoleId; // Use seeded role ID
            var role = await _repository.GetByIdAsync(roleId);
            role.Name = "UpdatedRole";

            await _repository.UpdateAsync(role);
            var updatedRole = await _repository.GetByIdAsync(roleId);

            Assert.Equal("UpdatedRole", updatedRole.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteRole()
        {
            var roleId = SeedingConstants.AdminRoleId; // Use seeded role ID
            await _repository.DeleteAsync(roleId);

            var deletedRole = await _repository.GetByIdAsync(roleId);
            Assert.Null(deletedRole);
        }
    }
}
