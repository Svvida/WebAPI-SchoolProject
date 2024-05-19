using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Types
{
    public class AccountRoleType : ObjectType<Users_Accounts_Roles>
    {
        protected override void Configure(IObjectTypeDescriptor<Users_Accounts_Roles> descriptor)
        {
            descriptor.Field(uar => uar.AccountId).Type<NonNullType<IdType>>();
            descriptor.Field(uar => uar.Account)
                .Type<AccountType>()
                .ResolveWith<AccountRoleResolvers>(r => r.GetAccount(default!, default!))
                .UseDbContext<UniversityContext>()
                .Name("account");

            descriptor.Field(uar => uar.RoleId).Type<NonNullType<IdType>>();
            descriptor.Field(uar => uar.Role)
                .Type<RoleType>()
                .ResolveWith<AccountRoleResolvers>(r => r.GetRole(default!, default!))
                .UseDbContext<UniversityContext>()
                .Name("role");
        }

        private class AccountRoleResolvers
        {
            public Users_Accounts GetAccount([Parent] Users_Accounts_Roles uar, [Service] IDbContextFactory<UniversityContext> dbContextFactory)
            {
                using var context = dbContextFactory.CreateDbContext();
                return context.Accounts
                    .Include(a => a.Roles)
                    .ThenInclude(uar => uar.Role)
                    .FirstOrDefault(a => a.Id == uar.AccountId);
            }

            public Roles GetRole([Parent] Users_Accounts_Roles uar, [Service] IDbContextFactory<UniversityContext> dbContextFactory)
            {
                using var context = dbContextFactory.CreateDbContext();
                return context.Roles
                    .FirstOrDefault(r => r.Id == uar.RoleId);
            }
        }
    }
}
