using System.ComponentModel.DataAnnotations;

namespace University.Domain.Entities
{
    public class Roles
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        // Navigation property for the junction table Users_Accounts_Roles
        public List<Users_Accounts_Roles> accounts { get; set; } = new List<Users_Accounts_Roles>();
    }
}
