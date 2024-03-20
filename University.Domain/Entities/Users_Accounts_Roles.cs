using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Domain.Entities
{
    public class Users_Accounts_Roles
    {
        public Guid account_id {  get; set; }
        public Users_Accounts account { get; set; }

        public Guid role_id { get; set; }
        public Roles role { get; set; }
    }
}
