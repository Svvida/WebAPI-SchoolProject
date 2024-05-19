using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace University.Domain.Entities
{
    public class Roles
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property for the junction table Users_Accounts_Roles
        public List<Users_Accounts_Roles> Accounts { get; set; } = new List<Users_Accounts_Roles>();
    }
}
