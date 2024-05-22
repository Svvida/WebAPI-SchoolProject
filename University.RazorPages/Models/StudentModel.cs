namespace University.RazorPages.Models
{
    public class StudentModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Pesel { get; set; }
        public string Gender { get; set; }
        public AddressModel Address { get; set; }
    }

    public class StudentInputModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Pesel { get; set; }
        public string Gender { get; set; }
        public AddressModel Address { get; set; }
    }

    public class UpdateStudentInputModel : StudentInputModel
    {
        public Guid Id { get; set; }
    }
}
