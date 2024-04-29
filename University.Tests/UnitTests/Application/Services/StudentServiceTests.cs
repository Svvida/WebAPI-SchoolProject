using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities;
using University.Domain.Interfaces;
using University.Domain.Enums;
using University.Application.DTOs;
using AutoMapper;
using University.Application.Services;
using AutoFixture;
using FluentAssertions;

namespace University.Tests.UnitTests.Application.Services
{
    public class StudentServiceTests
    {
        private Mock<IStudentRepository> mockRepo;
        private Mock<IMapper> mockMapper;
        private StudentService service;
        private Fixture fixture;

        public StudentServiceTests()
        {
            mockRepo = new Mock<IStudentRepository>();
            mockMapper = new Mock<IMapper>();
            fixture = new Fixture();
            service = new StudentService(mockRepo.Object, mockMapper.Object);

            // We are filling properties connected by relationships with default values like "null"
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b=>fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetAllStudentsAsync_ReturnsCorrectNumberOfStudents()
        {
            // Arrange
            var students = fixture.CreateMany<Students>(5).ToList();
            var studentDtos = fixture.CreateMany<StudentDto>(5).ToList();

            mockRepo.Setup(repo => repo.GetAllStudentsAsync())
                .ReturnsAsync(students);
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<StudentDto>>(It.IsAny<IEnumerable<Students>>()))
                .Returns(studentDtos);

            var result = await service.GetAllStudentsAsync();

            result.Should().HaveCount(5);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetStudentByIdAsync_VariousScenarios(bool studentExist)
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var student = studentExist ? new Students { id = studentId, name= "Marcin" } : null;
            var studentDto = studentExist ? new StudentDto { Id = studentId, Name = "Marcin" } : null;

            mockRepo.Setup(repo=> repo.GetStudentByIdAsync(studentId))
                .ReturnsAsync(student);
            mockMapper.Setup(mapper => mapper.Map<StudentDto>(student))
                .Returns(studentDto);

            // Act
            var result = studentExist ? await service.GetStudentByIdAsync(studentId) : null;

            // Assert
            if (studentExist)
            {
                result.Should().NotBeNull();
                result.Id.Should().Be(studentId);
                result.Name.Should().Be("Marcin");
            }
            else
            {
                await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetStudentByIdAsync(studentId));
            }
        }
    }
}
