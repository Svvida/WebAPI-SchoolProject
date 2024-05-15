using System.Security;
using University.Domain.Entities;
using University.GraphQL.Queries;
using University.Infrastructure.Data;

namespace University.GraphQL.Schemas
{
    public class AppQuery
    {
        private readonly StudentQuery _studentQuery = new StudentQuery();
        private readonly AddressQuery _addressQuery = new AddressQuery();
        private readonly AccountQuery _accountQuery = new AccountQuery();
        private readonly RoleQuery _roleQuery = new RoleQuery();
        private readonly AccountRoleQuery _accountRoleQuery = new AccountRoleQuery();

        // Student Queries
        public IQueryable<Students> AllStudents([Service] UniversityContext conetxt) =>
            _studentQuery.GetStudents(conetxt);
        public IQueryable<Students> StudentByField(string field,string value, [Service] UniversityContext conetxt) =>
            _studentQuery.GetStudentByField(field,value, conetxt);

        // Address Queries
        public IQueryable<Students_Addresses> AllAddresses([Service] UniversityContext conetxt) =>
            _addressQuery.GetAllAddresses(conetxt);

        public IQueryable<Students_Addresses> AddressByField(string field, string value,[Service] UniversityContext context) =>
            _addressQuery.GetAddressByField(field, value, context);

        // Account Queries
        public IQueryable<Users_Accounts> AllAccounts([Service] UniversityContext conetxt) =>
            conetxt.Accounts;

        public IQueryable<Users_Accounts> AccountByField(string field, string value,[Service] UniversityContext context) =>
            _accountQuery.GetAccountByField(field, value, context);

        // Role Queries
        public IQueryable<Roles> AllRoles([Service] UniversityContext conetxt) =>
            _roleQuery.GetAllRoles(conetxt);

        // AccountRole Queries
        public IQueryable<Users_Accounts_Roles> AllAccountsWithRoles([Service] UniversityContext conetxt) =>
            _accountRoleQuery.GetAllAccountsWithRoles(conetxt);
    }
}
