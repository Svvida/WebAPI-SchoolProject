using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Mappers;
using University.Application.Services;
using University.Domain.Interfaces;
using University.Infrastructure.Repositories;

namespace University.Tests.IntegrationTests.Controllers
{
    public class StudentsControllerTests : IntegrationTestBase
    {
        [Fact]
        public async Task GetAllStudents_ReturnsSeededStudents()
        {
            // Create an instance of the repository
            IStudentRepository studentRepository = new StudentRepository(context);

            // Create service with the repository
            var studentService = new StudentService(studentRepository, new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper());

            // Call the service method and perform assertion
            var students = await studentService.GetAllStudentsAsync();
            Assert.NotEmpty(students);
        }
    }
}
