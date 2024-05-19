using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace University.Domain.Entities
{
    public class Users_Accounts
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(72, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [AllowNull]
        public DateTime? DeactivationDate { get; set; }

        // Navigation property for the junction table Users_Accounts_Roles
        public List<Users_Accounts_Roles> Roles { get; set; } = new List<Users_Accounts_Roles>();

        // Example business logic
        public void Deactivate()
        {
            IsActive = false;
            DeactivationDate = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            DeactivationDate = null;
        }
    }
}
