using University.Domain.Entities;

namespace University.GraphQL.Types
{
    public class AddressType : ObjectType<Students_Addresses>
    {
        protected override void Configure(IObjectTypeDescriptor<Students_Addresses> descriptor)
        {
            descriptor.Field(a => a.id).Type<NonNullType<IdType>>();
            descriptor.Field(a => a.country).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.city).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.postal_code).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.street).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.building_number).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.apartment_number).Type<NonNullType<StringType>>();
        }
    }
}
