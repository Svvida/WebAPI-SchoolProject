using System;
using System.ComponentModel.DataAnnotations;
using University.Domain.Enums;

namespace University.Domain.Entities
{
    public class Students
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 2)]
        public string Surname { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(11)]
        [RegularExpression(@"^\d{11}$")]
        public string Pesel { get; set; }

        // Enum defined in another folder
        public Gender Gender { get; set; }

        // Relations
        public Guid? AddressId { get; set; }
        public Students_Addresses? Address { get; set; } // Navigation property

        public Guid? AccountId { get; set; }
        public Users_Accounts? Account { get; set; }

        // Example business logic
        public int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        public void ChangeAddress(Students_Addresses newAddress)
        {
            Address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
        }
    }
}
