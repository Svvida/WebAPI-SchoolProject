using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Mappers;
using University.Application.Services;
using University.Infrastructure.Repositories;
using Xunit;

namespace University.Tests.IntegrationTests.Services
{
    public class StudentServiceTests : IntegrationTestBase
    {
        private readonly StudentService _studentService;

        public StudentServiceTests() : base() // call base constructor to setup context
        {
            var studentRepository = new StudentRepository(context);
            _studentService = new StudentService(studentRepository, mapper);
        }

        [Fact]
        public async Task AddStudentAsync_StudentIsAdded()
        {
            // Arrange
            var studentDto = fixture.Create<StudentDto>();

            // Act
            await _studentService.AddStudentAsync(studentDto);

            // Assert
            var addedStudent = context.Students.Include(s => s.Address).FirstOrDefault(s => s.Pesel == studentDto.Pesel);
            addedStudent.Should().NotBeNull();
            addedStudent.Address.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateStudentAsync_StudentIsUpdated()
        {
            // Arrange
            var studentDto = fixture.Create<StudentDto>();
            await _studentService.AddStudentAsync(studentDto);

            // Retrieve the student with no tracking
            var updatedStudentDto = mapper.Map<StudentDto>(await context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Pesel == studentDto.Pesel));
            updatedStudentDto.Name = "Ambroży";

            // Act
            await _studentService.UpdateStudentAsync(updatedStudentDto);

            // Assert
            var updatedStudent = await context.Students.FirstOrDefaultAsync(s => s.Pesel == updatedStudentDto.Pesel);
            updatedStudent.Should().NotBeNull();
            updatedStudent.Name.Should().Be("Ambroży");
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
            var deletedStudent = context.Students.FirstOrDefault(s => s.Id == studentDto.Id);
            deletedStudent.Should().BeNull();
        }
    }
}
