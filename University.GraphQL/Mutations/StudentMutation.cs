using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Mutations
{
    public class StudentMutation
    {
        public async Task<Students> AddStudentAsync(StudentInput input, [Service] UniversityContext context)
        {
            var student = new Students
            {
                id = Guid.NewGuid(),
                name = input.Name,
                surname = input.Surname,
                date_of_birth = input.DateOfBirth,
                pesel = input.Pesel,
                gender = input.Gender,
                address_id = input.AddressId,
                account_id = input.AccountId,
            };

            context.Students.Add(student);
            await context.SaveChangesAsync();
            return student;
        }
    }
}
