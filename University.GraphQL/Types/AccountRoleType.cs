using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Types
{
    public class AccountRoleType : ObjectType<Users_Accounts_Roles>
    {
        protected override void Configure(IObjectTypeDescriptor<Users_Accounts_Roles> descriptor)
        {
            descriptor.Field(uar => uar.account_id).Type<IdType>();
            descriptor.Field(uar => uar.account)
                .Type<AccountType>()
                .ResolveWith<AccountRoleResolver>(r => r.GetAccount(default,default))
                .UseDbContext<UniversityContext>();

            descriptor.Field(uar => uar.role_id).Type<IdType>();
            descriptor.Field(uar => uar.role)
                .Type<RoleType>()
                .ResolveWith<AccountRoleResolver>(r => r.GetRole(default,default))
                .UseDbContext<UniversityContext>();
        }

        private class AccountRoleResolver
        {
            public Roles GetRole([Parent] Users_Accounts_Roles uar, [Service(ServiceKind.Resolver)] UniversityContext context)
            {
                return context.Roles.FirstOrDefault(r => r.id == uar.role_id);
            }

            public Users_Accounts GetAccount([Parent] Users_Accounts_Roles uar, [Service(ServiceKind.Resolver)] UniversityContext context)
            {
                return context.Accounts.FirstOrDefault(a => a.id == uar.account_id);
            }
        }
    }
}
