using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.Domain.Enums;
using University.Infrastructure.Data;

namespace University.GraphQL.Mutations
{
    public class StudentInput
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Pesel { get; set; }
        public Gender Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public Guid? AccountId { get; set; }
    }

    public class UpdateStudentInput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Pesel { get; set; }
        public Gender Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public Guid? AccountId { get; set; }
    }

    public class StudentMutation
    {
        public async Task<Students> AddStudentAsync(StudentInput input, [Service] UniversityContext context)
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
                address = address,
                account_id = input.AccountId
            };

            context.Students.Add(student);
            await context.SaveChangesAsync();
            return student;
        }

        public async Task<Students> UpdateStudentAsync(UpdateStudentInput input, [Service] UniversityContext context)
        {
            var student = await context.Students.Include(s => s.address).FirstOrDefaultAsync(s => s.id == input.Id);
            if (student == null)
            {
                throw new Exception($"Student with ID {input.Id} not found");
            }

            student.name = input.Name;
            student.surname = input.Surname;
            student.date_of_birth = input.DateOfBirth;
            student.pesel = input.Pesel;
            student.gender = input.Gender;
            student.account_id = input.AccountId;

            student.address.country = input.Country;
            student.address.city = input.City;
            student.address.postal_code = input.PostalCode;
            student.address.street = input.Street;
            student.address.building_number = input.BuildingNumber;
            student.address.apartment_number = input.ApartmentNumber;

            context.Students.Update(student);
            await context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudentAsync(Guid id, [Service] UniversityContext context)
        {
            var student = await context.Students.FindAsync(id);
            if (student == null)
            {
                throw new Exception($"Student with ID {id} not found");
            }

            context.Students.Remove(student);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
