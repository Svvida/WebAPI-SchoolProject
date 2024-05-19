using HotChocolate.Execution.Configuration;
using University.GraphQL.Types;

namespace University.GraphQL.Schemas
{
    public static class AppType
    {
        public static IRequestExecutorBuilder RegisterTypes(this IRequestExecutorBuilder builder)
        {
            return builder
                .AddType<AccountRoleType>()
                .AddType<RoleType>()
                .AddType<AccountType>()
                .AddType<StudentType>()
                .AddType<AddressType>()
                .AddType<GenderType>();
        }
    }
}
