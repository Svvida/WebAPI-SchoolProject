using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Queries
{
    public class StudentQuery
    {
        public IQueryable<Students> GetStudents([Service] UniversityContext context) => context.Students;
    }
}
