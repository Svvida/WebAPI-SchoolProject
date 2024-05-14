using University.Domain.Entities;
using University.GraphQL.Mutations;
using University.Infrastructure.Data;

namespace University.GraphQL.Schemas
{
    public class AppMutation
    {
        private readonly StudentMutation _studentMutation = new StudentMutation();

        public Task<Students> AddStudentAsync(StudentInput input, [Service] UniversityContext context) =>
            _studentMutation.AddStudentAsync(input, context);
    }
}
