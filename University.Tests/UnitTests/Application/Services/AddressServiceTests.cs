using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Mappers;
using University.Application.Services;
using University.Domain.Entities;
using University.Domain.Interfaces;
using Xunit;

namespace University.Tests.UnitTests.Application.Services
{
    public class AddressServiceTests
    {
        private readonly Mock<IAddressRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly AddressService _service;

        public AddressServiceTests()
        {
            _mockRepo = new Mock<IAddressRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
            _service = new AddressService(_mockRepo.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAddressesAsync_ReturnsAllAddresses()
        {
            var addresses = new List<Students_Addresses>
            {
                new Students_Addresses { Id = Guid.NewGuid(), Country = "Country1", City = "City1", PostalCode = "12345", Street = "Street1", BuildingNumber = "1" },
                new Students_Addresses { Id = Guid.NewGuid(), Country = "Country2", City = "City2", PostalCode = "67890", Street = "Street2", BuildingNumber = "2" }
            };
            _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(addresses);

            var result = await _service.GetAllAddressesAsync();

            Assert.Equal(addresses.Count, result.Count());
        }

        [Fact]
        public async Task GetAddressByIdAsync_ReturnsAddress_IfExists()
        {
            var addressId = Guid.NewGuid();
            var address = new Students_Addresses { Id = addressId, Country = "Country", City = "City", PostalCode = "12345", Street = "Street", BuildingNumber = "1" };
            _mockRepo.Setup(x => x.GetByIdAsync(addressId)).ReturnsAsync(address);

            var result = await _service.GetAddressByIdAsync(addressId);

            Assert.NotNull(result);
            Assert.Equal(address.City, result.City);
        }

        [Fact]
        public async Task CreateAddressAsync_AddsAddress()
        {
            var addressDto = new AddressDto { Country = "Country", City = "City", PostalCode = "12345", Street = "Street", BuildingNumber = "1" };
            var address = _mapper.Map<Students_Addresses>(addressDto);
            _mockRepo.Setup(x => x.CreateAsync(It.IsAny<Students_Addresses>())).ReturnsAsync(address);

            await _service.CreateAddressAsync(addressDto);

            _mockRepo.Verify(x => x.CreateAsync(It.IsAny<Students_Addresses>()), Times.Once());
        }

        [Fact]
        public async Task UpdateAddressAsync_UpdatesAddress()
        {
            var addressDto = new AddressDto { Id = Guid.NewGuid(), Country = "UpdatedCountry", City = "UpdatedCity", PostalCode = "54321", Street = "UpdatedStreet", BuildingNumber = "2" };
            var address = _mapper.Map<Students_Addresses>(addressDto);
            _mockRepo.Setup(x => x.GetByIdAsync(addressDto.Id)).ReturnsAsync(address);

            await _service.UpdateAddressAsync(addressDto);

            _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<Students_Addresses>()), Times.Once());
        }

        [Fact]
        public async Task DeleteAddressAsync_DeletesAddress()
        {
            var addressId = Guid.NewGuid();
            var address = new Students_Addresses { Id = addressId, Country = "Country", City = "City", PostalCode = "12345", Street = "Street", BuildingNumber = "1" };
            _mockRepo.Setup(x => x.GetByIdAsync(addressId)).ReturnsAsync(address);

            await _service.DeleteAddressAsync(addressId);

            _mockRepo.Verify(x => x.DeleteAsync(addressId), Times.Once());
        }
    }
}
