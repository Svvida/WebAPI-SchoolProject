using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
