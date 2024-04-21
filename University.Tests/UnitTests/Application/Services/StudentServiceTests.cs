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

        public StudentServiceTests()
        {
            mockRepo = new Mock<IStudentRepository>();
            mockMapper = new Mock<IMapper>();
            service = new StudentService(mockRepo.Object, mockMapper.Object);

            var fixture = new Fixture();

            // We are filling properties connected by relationships with default values like "null"
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b=>fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var students = fixture.CreateMany<Students>(2).ToList();
            var studentDtos = fixture.CreateMany<StudentDto>(2).ToList();

            mockRepo.Setup(repo => repo.GetAllStudentsAsync())
                .ReturnsAsync(students);

            mockMapper.Setup(mapper => mapper.Map<IEnumerable<StudentDto>>(It.IsAny<IEnumerable<Students>>()))
                .Returns(studentDtos);
        }

        [Fact]
        public async Task GetAllStudentsAsync_ReturnsCorrectNumberOfStudents()
        {
            var result = await service.GetAllStudentsAsync();

            result.Should().HaveCount(2);
        }
    }
}
