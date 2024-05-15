using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Queries
{
    public class AccountRoleQuery
    {
        public IQueryable<Users_Accounts_Roles> GetAllAccountsWithRoles(UniversityContext context)
        {
            return context.UserAccountRoles;
        }
    }
}
