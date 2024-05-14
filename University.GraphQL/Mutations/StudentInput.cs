using University.Domain.Enums;

namespace University.GraphQL.Mutations
{
    public class StudentInput
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Pesel { get; set; }
        public Gender Gender { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? AccountId { get; set; }
    }
}
