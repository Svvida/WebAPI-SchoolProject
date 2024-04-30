using AutoMapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;
using University.Application.Mappers;
using University.Application.Services;
using University.Domain.Entities;
using University.Infrastructure.Repositories;

namespace University.Tests.IntegrationTests.Services
{
    public class StudentServiceTests : IntegrationTestBase
    {
        private readonly StudentService _studentService;
        private readonly IMapper _mapper;

        public StudentServiceTests() : base() // call base constructor to setup context
        {
            var studentRepository = new StudentRepository(context);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
            _studentService = new StudentService(studentRepository, _mapper);
        }

        [Fact]
        public async Task AddStudentAsync_StudentIsAdded()
        {
            // Arrange
            var student = new Students
            {
                id = Guid.NewGuid(),
                name = "Jan",
                surname = "Kowalski",
                date_of_birth = new DateTime(1990, 1, 1),
                pesel = "12345678901",
                address = new Students_Addresses
                {
                    country = "Poland",
                    city = "Warsaw",
                    postal_code = "00-001",
                    street = "Marszałkowska",
                    building_number = "1",
                    apartment_number = "21"
                }
            };

            // Act
            var studentDto = _mapper.Map<StudentDto>(student);
            await _studentService.AddStudentAsync(studentDto);

            // Assert
            context.Students.Any(s => s.id == studentDto.Id).Should().BeTrue();
        }
    }
}
