using University.Domain.Entities;
using University.GraphQL.Queries;
using University.Infrastructure.Data;

namespace University.GraphQL.Schemas
{
    public class AppQuery
    {
        private readonly StudentQuery _studentQuery = new StudentQuery();

        public IQueryable<Students> GetStudent([Service] UniversityContext conetxt) =>
            _studentQuery.GetStudents(conetxt);
    }
}
