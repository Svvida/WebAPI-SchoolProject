using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace University.Domain.Entities
{
    public class Users_Accounts
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string login { get; set; }
        [Required]
        [StringLength(72, MinimumLength = 8)]
        public string password { get; set; }
        [Required]
        public bool is_active { get; set; }
        [AllowNull]
        public DateTime? deactivation_date { get; set; }
        // Navigation property for the junction table Users_Accounts_Roles
        public List<Users_Accounts_Roles> roles { get; set; } = new List<Users_Accounts_Roles>();

    }
}
