using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using University.Domain.Entities;
using University.Infrastructure.Repositories;
using Xunit;

namespace University.Tests.IntegrationTests.Repositories
{
    public class StudentRepositoryTests : IntegrationTestBase
    {
        private readonly StudentRepository _repository;

        public StudentRepositoryTests()
        {
            _repository = new StudentRepository(context);
        }

        [Fact]
        public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
        {
            var students = await _repository.GetAllStudentsAsync();
            Assert.NotEmpty(students);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ShouldReturnStudent_IfExists()
        {
            var studentId = new Guid("10000000-0000-0000-0000-000000000001"); // Use seeded student ID
            var student = await _repository.GetStudentByIdAsync(studentId);
            Assert.NotNull(student);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ShouldThrowException_IfNotExists()
        {
            var studentId = Guid.NewGuid();
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetStudentByIdAsync(studentId));
        }

        [Fact]
        public async Task AddStudentAsync_ShouldAddStudent()
        {
            var student = new Students
            {
                Id = Guid.NewGuid(),
                Name = "New",
                Surname = "Student",
                DateOfBirth = new DateTime(2001, 1, 1),
                Pesel = "12345678902",
                Gender = Domain.Enums.Gender.Male,
                Address = new Students_Addresses
                {
                    Id = Guid.NewGuid(),
                    Country = "Poland",
                    City = "Warsaw",
                    PostalCode = "00-001",
                    Street = "Another Street",
                    BuildingNumber = "2",
                    ApartmentNumber = "2"
                }
            };

            await _repository.AddStudentAsync(student);
            var students = await _repository.GetAllStudentsAsync();
            Assert.Contains(students, s => s.Name == "New" && s.Surname == "Student");
        }

        [Fact]
        public async Task UpdateStudentAsync_ShouldUpdateStudent()
        {
            var studentId = new Guid("10000000-0000-0000-0000-000000000001"); // Use seeded student ID
            var student = await _repository.GetStudentByIdAsync(studentId);
            student.Name = "Updated";

            await _repository.UpdateStudentAsync(student);
            var updatedStudent = await _repository.GetStudentByIdAsync(studentId);

            Assert.Equal("Updated", updatedStudent.Name);
        }

        [Fact]
        public async Task DeleteStudentAsync_ShouldDeleteStudent()
        {
            var studentId = new Guid("10000000-0000-0000-0000-000000000001"); // Use seeded student ID
            await _repository.DeleteStudentAsync(studentId);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetStudentByIdAsync(studentId));
        }
    }
}
