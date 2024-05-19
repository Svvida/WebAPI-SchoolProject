using System;
using FluentAssertions;
using University.Domain.Entities;
using Xunit;

namespace University.Tests.UnitTests.Domain.Entities
{
    public class Students_AddressesTests
    {
        [Fact]
        public void Constructor_InitializesStudentsAddressesWithValidData()
        {
            // Arrange
            var country = "Poland";
            var city = "Kraków";
            var postalCode = "30-001";
            var street = "Main Street";
            var buildingNumber = "1";
            var apartmentNumber = "10";

            // Act
            var address = new Students_Addresses
            {
                Id = Guid.NewGuid(),
                Country = country,
                City = city,
                PostalCode = postalCode,
                Street = street,
                BuildingNumber = buildingNumber,
                ApartmentNumber = apartmentNumber
            };

            // Assert
            address.Country.Should().Be(country);
            address.City.Should().Be(city);
            address.PostalCode.Should().Be(postalCode);
            address.Street.Should().Be(street);
            address.BuildingNumber.Should().Be(buildingNumber);
            address.ApartmentNumber.Should().Be(apartmentNumber);
        }
    }
}
