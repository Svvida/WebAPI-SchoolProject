using Moq;
using Xunit;
using University.Application.DTOs;
using University.Application.Services;
using Microsoft.AspNetCore.Mvc;
using University.RestApi.Controllers;
using FluentAssertions;
using University.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace University.Tests.IntegrationTests.Controllers
{
    public class AddressesControllerTests : IntegrationTestBase
    {
        private readonly Mock<IAddressService> _mockAddressService;
        private readonly AddressesController _controller;

        public AddressesControllerTests()
        {
            _mockAddressService = new Mock<IAddressService>();
            _controller = new AddressesController(_mockAddressService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfAddresses()
        {
            // Arrange
            var addresses = new List<AddressDto>
            {
                new AddressDto { Id = Guid.NewGuid(), City = "City1", Street = "Street1", BuildingNumber = "1" },
                new AddressDto { Id = Guid.NewGuid(), City = "City2", Street = "Street2", BuildingNumber = "2" }
            };
            _mockAddressService.Setup(service => service.GetAllAddressesAsync()).ReturnsAsync(addresses);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(addresses);
        }

        [Fact]
        public async Task Get_ReturnsAddress_WhenAddressExists()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var address = new AddressDto { Id = addressId, City = "City", Street = "Street", BuildingNumber = "1" };
            _mockAddressService.Setup(service => service.GetAddressByIdAsync(addressId)).ReturnsAsync(address);

            // Act
            var result = await _controller.Get(addressId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(address);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            _mockAddressService.Setup(service => service.GetAddressByIdAsync(addressId)).ReturnsAsync((AddressDto)null);

            // Act
            var result = await _controller.Get(addressId);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Create_ReturnsCreatedAddress()
        {
            // Arrange
            var newAddress = new AddressDto { City = "NewCity", Street = "NewStreet", BuildingNumber = "1" };
            var createdAddress = new AddressDto { Id = Guid.NewGuid(), City = "NewCity", Street = "NewStreet", BuildingNumber = "1" };
            _mockAddressService.Setup(service => service.CreateAddressAsync(newAddress)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(newAddress);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.Value.Should().BeEquivalentTo(newAddress);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenAddressIsUpdated()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var updatedAddress = new AddressDto { Id = addressId, City = "UpdatedCity", Street = "UpdatedStreet", BuildingNumber = "2" };

            _mockAddressService.Setup(service => service.GetAddressByIdAsync(addressId)).ReturnsAsync(updatedAddress);
            _mockAddressService.Setup(service => service.UpdateAddressAsync(updatedAddress)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(addressId, updatedAddress);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var updatedAddress = new AddressDto { Id = Guid.NewGuid(), City = "UpdatedCity", Street = "UpdatedStreet", BuildingNumber = "2" };

            // Act
            var result = await _controller.Update(addressId, updatedAddress);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenAddressIsDeleted()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var existingAddress = new AddressDto { Id = addressId, City = "City", Street = "Street", BuildingNumber = "1" };

            _mockAddressService.Setup(service => service.GetAddressByIdAsync(addressId)).ReturnsAsync(existingAddress);
            _mockAddressService.Setup(service => service.DeleteAddressAsync(addressId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(addressId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            _mockAddressService.Setup(service => service.GetAddressByIdAsync(addressId)).ReturnsAsync((AddressDto)null);

            // Act
            var result = await _controller.Delete(addressId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
