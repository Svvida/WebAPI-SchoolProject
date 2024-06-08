using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Domain.Entities;
using University.Infrastructure.Data;
using University.Infrastructure.Repositories;
using Xunit;

namespace University.Tests.IntegrationTests.Repositories
{
    public class AddressRepositoryTests : IntegrationTestBase
    {
        private readonly AddressRepository _repository;

        public AddressRepositoryTests()
        {
            _repository = new AddressRepository(context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAddresses()
        {
            var addresses = await _repository.GetAllAsync();
            Assert.NotEmpty(addresses);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAddress_IfExists()
        {
            var addressId = SeedingConstants.TestAddressId; // Use seeded address ID
            var address = await _repository.GetByIdAsync(addressId);
            Assert.NotNull(address);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_IfNotExists()
        {
            var addressId = Guid.NewGuid();
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetByIdAsync(addressId));
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAddress()
        {
            var address = new Students_Addresses
            {
                Id = Guid.NewGuid(),
                Country = "Poland",
                City = "Warsaw",
                PostalCode = "00-001",
                Street = "New Street",
                BuildingNumber = "10",
                ApartmentNumber = "5"
            };

            await _repository.CreateAsync(address);
            var addresses = await _repository.GetAllAsync();
            Assert.Contains(addresses, a => a.Street == "New Street");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAddress()
        {
            var addressId = SeedingConstants.TestAddressId; // Use seeded address ID
            var address = await _repository.GetByIdAsync(addressId);
            address.Street = "Updated Street";

            await _repository.UpdateAsync(address);
            var updatedAddress = await _repository.GetByIdAsync(addressId);

            Assert.Equal("Updated Street", updatedAddress.Street);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAddress()
        {
            var addressId = SeedingConstants.TestAddressId; // Use seeded address ID
            await _repository.DeleteAsync(addressId);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetByIdAsync(addressId));
        }
    }
}
