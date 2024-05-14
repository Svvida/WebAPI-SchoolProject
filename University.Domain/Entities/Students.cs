using System.ComponentModel.DataAnnotations;
using University.Domain.Enums;

namespace University.Domain.Entities
{
    public class Students
    {
        public Guid id { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 2)]
        public string name { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 2)]
        public string surname { get; set; }

        [Required]
        public DateTime date_of_birth { get; set; }

        [Required]
        [StringLength(11)]
        [RegularExpression(@"^\d{11}$")]
        public string pesel { get; set; }

        // Enum defined in other folder
        public Gender gender { get; set; }

        // Relations
        public Guid? address_id { get; set; }
        public Students_Addresses? address { get; set; } // Navigation property

        public Guid? account_id { get; set; }
        public Users_Accounts? account { get; set; }
    }
}
