using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Types
{
    public class AccountType : ObjectType<Users_Accounts>
    {
        protected override void Configure(IObjectTypeDescriptor<Users_Accounts> descriptor)
        {
            descriptor.Field(ua => ua.Id).Type<NonNullType<IdType>>();
            descriptor.Field(ua => ua.Email).Type<NonNullType<StringType>>();
            descriptor.Field(ua => ua.Login).Type<NonNullType<StringType>>();
            descriptor.Field(ua => ua.Password).Type<NonNullType<StringType>>();
            descriptor.Field(ua => ua.IsActive).Type<NonNullType<BooleanType>>();
            descriptor.Field(ua => ua.DeactivationDate).Type<DateType>();

            descriptor.Field(ua => ua.Roles)
                .Type<ListType<AccountRoleType>>()
                .ResolveWith<AccountResolver>(r => r.GetUar(default!, default!))
                .UseDbContext<UniversityContext>();
        }

        private class AccountResolver
        {
            public IQueryable<Users_Accounts_Roles> GetUar([Parent] Users_Accounts account, [Service] IDbContextFactory<UniversityContext> dbContextFactory)
            {
                using var context = dbContextFactory.CreateDbContext();
                return context.UserAccountRoles.Where(uar => uar.AccountId == account.Id);
            }
        }
    }
}
