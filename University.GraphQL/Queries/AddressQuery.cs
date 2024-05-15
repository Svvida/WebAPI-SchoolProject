using HotChocolate.Validation;
using System.Linq.Expressions;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Queries
{
    public class AddressQuery
    {
        public IQueryable<Students_Addresses> GetAllAddresses(UniversityContext context)
        {
            return context.Addresses;
        }

        public IQueryable<Students_Addresses> GetAddressByField(string field,string value, UniversityContext context)
        {
            var parameter = Expression.Parameter(typeof(Students_Addresses), "a");
            var property = Expression.Property(parameter, field);
            var constant = Expression.Constant(value);
            var equals = Expression.Equal(property, constant);
            var predicate = Expression.Lambda<Func<Students_Addresses, bool>>(equals, parameter);

            return context.Addresses.Where(predicate);
        }
    }
}
