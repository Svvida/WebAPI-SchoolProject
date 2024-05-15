using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Queries
{
    public class RoleQuery
    {
        public IQueryable<Roles> GetAllRoles(UniversityContext context)
        {
            return context.Roles;
        }
    }
}
