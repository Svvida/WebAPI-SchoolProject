using System.Linq.Expressions;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Queries
{
    public class AccountQuery
    {
        public IQueryable<Users_Accounts> GetAllAccounts(UniversityContext context)
        {
            return context.Accounts;
        }

        public IQueryable<Users_Accounts> GetAccountByField(string field, string value, UniversityContext context)
        {
            var parameter = Expression.Parameter(typeof(Users_Accounts), "ua");
            var property = Expression.Property(parameter, field);
            var constant = Expression.Constant(value);
            var equals = Expression.Equal(property, constant);
            var predicate = Expression.Lambda<Func<Users_Accounts, bool>>(equals, parameter);

            return context.Accounts.Where(predicate);
        } 
    }
}
