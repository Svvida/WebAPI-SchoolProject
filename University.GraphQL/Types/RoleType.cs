using University.Domain.Entities;

namespace University.GraphQL.Types
{
    public class RoleType : ObjectType<Roles>
    {
        protected override void Configure(IObjectTypeDescriptor<Roles> descriptor)
        {
            descriptor.Field(r => r.id).Type<NonNullType<IdType>>();
            descriptor.Field(r => r.name).Type<NonNullType<StringType>>();
            descriptor.Field(r => r.accounts).Type<AccountRoleType>();
        }
    }
}
