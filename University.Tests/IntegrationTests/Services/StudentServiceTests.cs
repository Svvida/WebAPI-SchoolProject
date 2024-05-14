using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
using University.Domain.Enums;
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
            var studentDto = fixture.Create<StudentDto>();

            // Act
            await _studentService.AddStudentAsync(studentDto);

            // Assert
            var addedStudent = context.Students.Include(s => s.address).FirstOrDefault(s => s.pesel == studentDto.Pesel);
            addedStudent.Should().NotBeNull();
            addedStudent.address.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateStudentAsync_StudentIsUpdated()
        {
            // Arrange
            var studentDto = fixture.Create<StudentDto>();
            await _studentService.AddStudentAsync(studentDto);

            // Retrieve the student with no tracking
            var updatedStudentDto = _mapper.Map<StudentDto>(await context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.pesel == studentDto.Pesel));
            updatedStudentDto.Name = "Ambroży"; 

            // Act
            await _studentService.UpdateStudentAsync(updatedStudentDto);

            // Assert
            var updatedStudent = await context.Students.FirstOrDefaultAsync(s => s.pesel == updatedStudentDto.Pesel);
            updatedStudent.Should().NotBeNull();
            updatedStudent.name.Should().Be("Ambroży");
        }

        [Fact]
        public async Task DeleteStudentAsync_StudentIsDeleted()
        {
            // Arrange
            var studentDto = fixture.Create<StudentDto>();
            await _studentService.AddStudentAsync(studentDto);

            // Act
            await _studentService.DeleteStudentAsync(studentDto.Id);

            // Assert
            var deletedStudent = context.Students.FirstOrDefault(s => s.id == studentDto.Id);
            deletedStudent.Should().BeNull();
        }

    }
}
