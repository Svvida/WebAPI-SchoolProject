using Moq;
using Xunit;
using University.Application.DTOs;
using University.Application.Services;
using Microsoft.AspNetCore.Mvc;
using University.RestApi.Controllers;
using FluentAssertions;
using University.Application.Interfaces;

namespace University.Tests.IntegrationTests.Controllers
{
    public class RolesControllerTests
    {
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly RolesController _controller;

        public RolesControllerTests()
        {
            _mockRoleService = new Mock<IRoleService>();
            _controller = new RolesController(_mockRoleService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfRoles()
        {
            // Arrange
            var roles = new List<RoleDto>
            {
                new RoleDto { Id = Guid.NewGuid(), Name = "Admin" },
                new RoleDto { Id = Guid.NewGuid(), Name = "User" }
            };
            _mockRoleService.Setup(service => service.GetAllRolesAsync()).ReturnsAsync(roles);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public async Task Get_ReturnsRole_WhenRoleExists()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var role = new RoleDto { Id = roleId, Name = "Admin" };
            _mockRoleService.Setup(service => service.GetRoleByIdAsync(roleId)).ReturnsAsync(role);

            // Act
            var result = await _controller.Get(roleId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(role);
        }

        [Fact]
        public async Task Create_ReturnsCreatedRole()
        {
            // Arrange
            var newRole = new CreateRoleDto { Name = "NewRole" };
            var createdRole = new RoleDto { Id = Guid.NewGuid(), Name = "NewRole" };
            _mockRoleService.Setup(service => service.AddRolesAsync(newRole)).ReturnsAsync(createdRole);

            // Act
            var result = await _controller.Create(newRole);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.Value.Should().BeEquivalentTo(createdRole);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenRoleIsUpdated()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var updatedRole = new RoleDto { Id = roleId, Name = "UpdatedRole" };

            _mockRoleService.Setup(service => service.UpdateRolesAsync(updatedRole)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(roleId, updatedRole);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenRoleIsDeleted()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            _mockRoleService.Setup(service => service.DeleteRolesAsync(roleId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(roleId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
