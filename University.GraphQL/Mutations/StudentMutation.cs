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
                Id = Guid.NewGuid(),
                Country = input.Country,
                City = input.City,
                PostalCode = input.PostalCode,
                Street = input.Street,
                BuildingNumber = input.BuildingNumber,
                ApartmentNumber = input.ApartmentNumber
            };

            var student = new Students
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Surname = input.Surname,
                DateOfBirth = input.DateOfBirth,
                Pesel = input.Pesel,
                Gender = input.Gender,
                Address = address,
                AccountId = input.AccountId
            };

            context.Students.Add(student);
            await context.SaveChangesAsync();
            return student;
        }

        public async Task<Students> UpdateStudentAsync(UpdateStudentInput input, [Service] UniversityContext context)
        {
            var student = await context.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.Id == input.Id);
            if (student == null)
            {
                throw new Exception($"Student with ID {input.Id} not found");
            }

            student.Name = input.Name;
            student.Surname = input.Surname;
            student.DateOfBirth = input.DateOfBirth;
            student.Pesel = input.Pesel;
            student.Gender = input.Gender;
            student.AccountId = input.AccountId;

            student.Address.Country = input.Country;
            student.Address.City = input.City;
            student.Address.PostalCode = input.PostalCode;
            student.Address.Street = input.Street;
            student.Address.BuildingNumber = input.BuildingNumber;
            student.Address.ApartmentNumber = input.ApartmentNumber;

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
