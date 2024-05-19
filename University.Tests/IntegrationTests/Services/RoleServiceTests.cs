using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using University.Application.DTOs;
using University.Application.Mappers;
using University.Application.Services;
using University.Domain.Entities;
using University.Domain.Interfaces;
using Xunit;

namespace University.Tests.IntegrationTests.Services
{
    public class RoleServiceTests : IntegrationTestBase
    {
        private readonly RoleService _roleService;
        private readonly IMapper _mapper;
        private readonly Mock<IRoleRepository> _mockRoleRepository;

        public RoleServiceTests()
        {
            _mockRoleRepository = new Mock<IRoleRepository>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
            _roleService = new RoleService(_mockRoleRepository.Object, _mapper);
        }

        [Fact]
        public async Task AddRole_ShouldAddRoleSuccessfully()
        {
            // Arrange
            var newRole = new CreateRoleDto { Name = "NewRole" };
            var createdRole = new Roles { Id = Guid.NewGuid(), Name = "NewRole" };

            _mockRoleRepository.Setup(repo => repo.AddAsync(It.IsAny<Roles>())).Returns(Task.FromResult(createdRole));

            // Act
            var result = await _roleService.AddRolesAsync(newRole);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("NewRole");
            _mockRoleRepository.Verify(repo => repo.AddAsync(It.IsAny<Roles>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRole_ShouldUpdateRoleSuccessfully()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var existingRole = new Roles { Id = roleId, Name = "ExistingRole" };
            var updatedRoleDto = new RoleDto { Id = roleId, Name = "UpdatedRole" };

            _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).Returns(Task.FromResult(existingRole));
            _mockRoleRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Roles>())).Returns(Task.CompletedTask);

            // Act
            await _roleService.UpdateRolesAsync(updatedRoleDto);

            // Assert
            _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
            _mockRoleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Roles>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRole_ShouldDeleteRoleSuccessfully()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var existingRole = new Roles {Id = roleId, Name = "RoleToDelete" };

            _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).Returns(Task.FromResult(existingRole));
            _mockRoleRepository.Setup(repo => repo.DeleteAsync(roleId)).Returns(Task.CompletedTask);

            // Act
            await _roleService.DeleteRolesAsync(roleId);

            // Assert
            _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
            _mockRoleRepository.Verify(repo => repo.DeleteAsync(roleId), Times.Once);
        }
    }
}
