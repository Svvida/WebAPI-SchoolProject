using University.GraphQL.Types;

namespace University.GraphQL.Schemas
{
    public class AppType
    {
        public static void RegisterTypes(ISchemaBuilder builder)
        {
            builder.AddType<AccountRoleType>();
            builder.AddType<RoleType>();
            builder.AddType<AccountType>();
            builder.AddType<StudentType>();
            builder.AddType<AddressType>();
            builder.AddType<EnumType<GenderType>>();
        }
    }
}
