using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Mutations
{
    public class AccountInput
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateAccountInput
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool IsActive { get; set; }
    }

    public class AccountMutation
    {
        public async Task<Users_Accounts> AddAccountAsync(AccountInput input, [Service] UniversityContext context)
        {
            var account = new Users_Accounts
            {
                id = Guid.NewGuid(),
                email = input.Email,
                login = input.Login,
                password = input.Password,  // Assume password is already hashed
                is_active = input.IsActive
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync();
            return account;
        }

        public async Task<Users_Accounts> UpdateAccountAsync(UpdateAccountInput input, [Service] UniversityContext context)
        {
            var account = await context.Accounts.FindAsync(input.Id);
            if (account == null)
            {
                throw new Exception($"Account with ID {input.Id} not found");
            }

            account.email = input.Email;
            account.login = input.Login;
            account.is_active = input.IsActive;

            context.Accounts.Update(account);
            await context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> DeleteAccountAsync(Guid id, [Service] UniversityContext context)
        {
            var account = await context.Accounts.FindAsync(id);
            if (account == null)
            {
                throw new Exception($"Account with ID {id} not found");
            }

            context.Accounts.Remove(account);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Users_Accounts_Roles> AddRoleToAccountAsync(Guid accountId, Guid roleId, [Service] UniversityContext context)
        {
            var account = await context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                throw new Exception($"Account with ID {accountId} not found");
            }

            var role = await context.Roles.FindAsync(roleId);
            if (role == null)
            {
                throw new Exception($"Role with ID {roleId} not found");
            }

            var accountRole = new Users_Accounts_Roles
            {
                account_id = accountId,
                role_id = roleId
            };

            context.UserAccountRoles.Add(accountRole);
            await context.SaveChangesAsync();
            return accountRole;
        }

        public async Task<bool> DeleteRoleFromAccountAsync(Guid accountId, Guid roleId, [Service] UniversityContext context)
        {
            var accountRole = await context.UserAccountRoles.FindAsync(accountId, roleId);
            if (accountRole == null)
            {
                throw new Exception($"Role assignment not found for Account ID {accountId} and Role ID {roleId}");
            }

            context.UserAccountRoles.Remove(accountRole);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
