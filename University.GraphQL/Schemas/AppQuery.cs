using System.Linq.Expressions;
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
        [GraphQLName("allStudents")]
        public IQueryable<Students> GetAllStudents([Service] UniversityContext context) =>
            _studentQuery.GetStudents(context);

        [GraphQLName("studentByField")]
        public IQueryable<Students> GetStudentByField(string field, string value, [Service] UniversityContext context) =>
            _studentQuery.GetStudentByField(field, value, context);

        // Address Queries
        [GraphQLName("allAddresses")]
        public IQueryable<Students_Addresses> GetAllAddresses([Service] UniversityContext context) =>
            _addressQuery.GetAllAddresses(context);

        [GraphQLName("addressByField")]
        public IQueryable<Students_Addresses> GetAddressByField(string field, string value, [Service] UniversityContext context) =>
            _addressQuery.GetAddressByField(field, value, context);

        // Account Queries
        [GraphQLName("allAccounts")]
        public IQueryable<Users_Accounts> GetAllAccounts([Service] UniversityContext context) =>
            _accountQuery.GetAllAccounts(context);

        [GraphQLName("accountByField")]
        public IQueryable<Users_Accounts> GetAccountByField(string field, string value, [Service] UniversityContext context) =>
            _accountQuery.GetAccountByField(field, value, context);

        // Role Queries
        [GraphQLName("allRoles")]
        public IQueryable<Roles> GetAllRoles([Service] UniversityContext context) =>
            _roleQuery.GetAllRoles(context);

        // AccountRole Queries
        [GraphQLName("allAccountsWithRoles")]
        public IQueryable<Users_Accounts_Roles> GetAllAccountsWithRoles([Service] UniversityContext context) =>
            _accountRoleQuery.GetAllAccountsWithRoles(context);
    }
}
