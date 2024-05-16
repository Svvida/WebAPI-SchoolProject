using University.Domain.Entities;
using University.Infrastructure.Data;
using HotChocolate;
using System.Threading.Tasks;

namespace University.GraphQL.Mutations
{
    public class StudentMutation
    {
        public async Task<Students> AddStudentAsync(StudentInput input, [Service(ServiceKind.Resolver)] UniversityContext context)
        {
            var address = new Students_Addresses
            {
                id = Guid.NewGuid(),
                country = input.Country,
                city = input.City,
                postal_code = input.PostalCode,
                street = input.Street,
                building_number = input.BuildingNumber,
                apartment_number = input.ApartmentNumber
            };

            var student = new Students
            {
                id = Guid.NewGuid(),
                name = input.Name,
                surname = input.Surname,
                date_of_birth = input.DateOfBirth,
                pesel = input.Pesel,
                gender = input.Gender,
                address_id = address.id,
                address = address,
                account_id = input.AccountId
            };

            context.Addresses.Add(address);
            context.Students.Add(student);
            await context.SaveChangesAsync();
            return student;
        }
    }
}
