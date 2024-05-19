using System;

namespace University.Domain.Entities
{
    public class Users_Accounts_Roles
    {
        public Guid AccountId { get; set; }
        public Users_Accounts Account { get; set; }

        public Guid RoleId { get; set; }
        public Roles Role { get; set; }
    }
}
