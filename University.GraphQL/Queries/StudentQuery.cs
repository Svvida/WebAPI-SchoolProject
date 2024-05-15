using System.Linq.Expressions;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Queries
{
    public class StudentQuery
    {
        public IQueryable<Students> GetStudents(UniversityContext context)
        {
            return context.Students;
        }

        public IQueryable<Students> GetStudentByField(string field, string value, UniversityContext context)
        {
            // Create a parameter expression for the Students type
            var parameter = Expression.Parameter(typeof(Students), "a");
            // Access specified field on the prarameter
            var property = Expression.Property(parameter, field);
            // Get type of the field
            var fieldType = property.Type;
            // Convert value to appropriate type
            object convertedValue;
            if (fieldType.IsEnum)
            {
                // Convert to enum type
                convertedValue = Enum.Parse(fieldType, value);
            }
            else
            {
                convertedValue = Convert.ChangeType(value, fieldType);
            }
            // Create constant expression with converted value
            var constant = Expression.Constant(convertedValue);
            // Create equality expression
            var equals = Expression.Equal(property, constant);
            // Create lambda expression a => a.field == value
            var predicate = Expression.Lambda<Func<Students, bool>>(equals, parameter);

            // Apply complete expression to filted the students by chosen field
            return context.Students.Where(predicate);
        }
    }
}
