using HotChocolate.Execution.Configuration;
using University.GraphQL.Types;

namespace University.GraphQL.Schemas
{
    public static class AppType
    {
        public static void RegisterTypes(this IRequestExecutorBuilder builder)
        {
            builder.AddType<AccountRoleType>();
            builder.AddType<RoleType>();
            builder.AddType<AccountType>();
            builder.AddType<StudentType>();
            builder.AddType<AddressType>();
            builder.AddType<GenderType>();
        }
    }
}
