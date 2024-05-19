using System;
using FluentAssertions;
using University.Domain.Entities;
using University.Domain.Enums;
using Xunit;

namespace University.Tests.UnitTests.Domain.Entities
{
    public class StudentTests
    {
        [Fact]
        public void Constructor_InitializesStudentWithValidData()
        {
            // Arrange
            var name = "John";
            var surname = "Doe";
            var dateOfBirth = new DateTime(2000, 1, 1);
            var pesel = "12345678901";
            var gender = Gender.Male;
            var address = new Students_Addresses
            {
                Country = "Poland",
                City = "Kraków",
                PostalCode = "30-001",
                Street = "Main Street",
                BuildingNumber = "1",
                ApartmentNumber = "10"
            };

            // Act
            var student = new Students
            {
                Id = Guid.NewGuid(),
                Name = name,
                Surname = surname,
                DateOfBirth = dateOfBirth,
                Pesel = pesel,
                Gender = gender,
                Address = address
            };

            // Assert
            student.Name.Should().Be(name);
            student.Surname.Should().Be(surname);
            student.DateOfBirth.Should().Be(dateOfBirth);
            student.Pesel.Should().Be(pesel);
            student.Gender.Should().Be(gender);
            student.Address.Should().Be(address);
        }

        [Fact]
        public void ChangeAddress_UpdatesAddress()
        {
            // Arrange
            var student = new Students
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
                DateOfBirth = new DateTime(2000, 1, 1),
                Pesel = "12345678901",
                Gender = Gender.Male,
                Address = new Students_Addresses()
            };
            var newAddress = new Students_Addresses
            {
                Country = "Poland",
                City = "Warsaw",
                PostalCode = "00-001",
                Street = "New Street",
                BuildingNumber = "2",
                ApartmentNumber = "20"
            };

            // Act
            student.ChangeAddress(newAddress);

            // Assert
            student.Address.Should().Be(newAddress);
        }

        [Fact]
        public void CalculateAge_ReturnsCorrectAge()
        {
            // Arrange
            var dateOfBirth = new DateTime(2000, 1, 1);
            var student = new Students
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
                DateOfBirth = dateOfBirth,
                Pesel = "12345678901",
                Gender = Gender.Male
            };

            // Act
            var age = student.CalculateAge();

            // Assert
            var expectedAge = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-expectedAge)) expectedAge--;
            age.Should().Be(expectedAge);
        }

        [Fact]
        public void ChangeAddress_ThrowsArgumentNullException_WhenNewAddressIsNull()
        {
            // Arrange
            var student = new Students
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
                DateOfBirth = new DateTime(2000, 1, 1),
                Pesel = "12345678901",
                Gender = Gender.Male,
                Address = new Students_Addresses()
            };

            // Act
            Action act = () => student.ChangeAddress(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'newAddress')");
        }
    }
}
