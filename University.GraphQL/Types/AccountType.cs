using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Types
{
    public class AccountType : ObjectType<Users_Accounts>
    {
        protected override void Configure(IObjectTypeDescriptor<Users_Accounts> descriptor)
        {
            descriptor.Field(ua => ua.id).Type<NonNullType<IdType>>();
            descriptor.Field(ua => ua.email).Type<NonNullType<StringType>>();
            descriptor.Field(ua => ua.login).Type<NonNullType<StringType>>();
            descriptor.Field(ua => ua.password).Type<NonNullType<StringType>>();
            descriptor.Field(ua => ua.is_active).Type<NonNullType<BooleanType>>();
            descriptor.Field(ua => ua.deactivation_date).Type<DateType>();

            descriptor.Field(ua => ua.roles)
                .Type<AccountRoleType>()
                .ResolveWith<AccountResolver>(r => r.GetUar)
                .UseDbContext<UniversityContext>();
        }

        private class AccountResolver
        {
            public Users_Accounts_Roles GetUar([Parent]  Users_Accounts account, [ScopedService] UniversityContext context)
            {
                return context.UserAccountRoles.FirstOrDefault(uar => uar.account_id == account.id);
            }
        }
    }
}
