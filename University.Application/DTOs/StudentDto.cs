using University.Domain.Enums;

namespace University.Application.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Pesel { get; set; }
        public Gender Gender { get; set; }
        public Guid AddressId { get; set; }
        public AddressDto Address { get; set; }
        public Guid AccountId { get; set; }

    }
}
