using System.Threading.Tasks;
using University.Domain.Entities;
using University.GraphQL.Mutations;
using University.Infrastructure.Data;

namespace University.GraphQL.Schemas
{
    public class AppMutation
    {
        private readonly StudentMutation _studentMutation = new StudentMutation();
        private readonly AccountMutation _accountMutation = new AccountMutation();
        private readonly RoleMutation _roleMutation = new RoleMutation();
        private readonly AddressMutation _addressMutation = new AddressMutation();

        // Student Mutations
        public Task<Students> AddStudentAsync(StudentInput input, [Service] UniversityContext context) =>
            _studentMutation.AddStudentAsync(input, context);

        public Task<Students> UpdateStudentAsync(UpdateStudentInput input, [Service] UniversityContext context) =>
            _studentMutation.UpdateStudentAsync(input, context);

        public Task<bool> DeleteStudentAsync(Guid id, [Service] UniversityContext context) =>
            _studentMutation.DeleteStudentAsync(id, context);

        // Account Mutations
        public Task<Users_Accounts> AddAccountAsync(AccountInput input, [Service] UniversityContext context) =>
            _accountMutation.AddAccountAsync(input, context);

        public Task<Users_Accounts> UpdateAccountAsync(UpdateAccountInput input, [Service] UniversityContext context) =>
            _accountMutation.UpdateAccountAsync(input, context);

        public Task<bool> DeleteAccountAsync(Guid id, [Service] UniversityContext context) =>
            _accountMutation.DeleteAccountAsync(id, context);

        // Role Mutations
        public Task<Roles> AddRoleAsync(RoleInput input, [Service] UniversityContext context) =>
            _roleMutation.AddRoleAsync(input, context);

        public Task<Roles> UpdateRoleAsync(Guid id, string name, [Service] UniversityContext context) =>
            _roleMutation.UpdateRoleAsync(id, name, context);

        public Task<bool> DeleteRoleAsync(Guid id, [Service] UniversityContext context) =>
            _roleMutation.DeleteRoleAsync(id, context);

        public Task<Users_Accounts_Roles> AddRoleToAccountAsync(Guid accountId, Guid roleId, [Service] UniversityContext context) =>
            _accountMutation.AddRoleToAccountAsync(accountId, roleId, context);

        public Task<bool> DeleteRoleFromAccountAsync(Guid accountId, Guid roleId, [Service] UniversityContext context) =>
            _accountMutation.DeleteRoleFromAccountAsync(accountId, roleId, context);

        // Address Mutations
        public Task<Students_Addresses> AddAddressAsync(AddressInput input, [Service] UniversityContext context) =>
            _addressMutation.AddAddressAsync(input, context);

        public Task<Students_Addresses> UpdateAddressAsync(UpdateAddressInput input, [Service] UniversityContext context) =>
            _addressMutation.UpdateAddressAsync(input, context);

        public Task<bool> DeleteAddressAsync(Guid id, [Service] UniversityContext context) =>
            _addressMutation.DeleteAddressAsync(id, context);
    }
}
