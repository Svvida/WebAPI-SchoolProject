using University.Domain.Entities;

namespace University.GraphQL.Types
{
    public class AddressType : ObjectType<Students_Addresses>
    {
        protected override void Configure(IObjectTypeDescriptor<Students_Addresses> descriptor)
        {
            descriptor.Field(a => a.Id).Type<NonNullType<IdType>>();
            descriptor.Field(a => a.Country).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.City).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.PostalCode).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.Street).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.BuildingNumber).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.ApartmentNumber).Type<NonNullType<StringType>>();
        }
    }
}
