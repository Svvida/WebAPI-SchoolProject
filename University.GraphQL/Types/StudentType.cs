using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Types
{
    public class StudentType : ObjectType<Students>
    {
        protected override void Configure(IObjectTypeDescriptor<Students> descriptor)
        {
            descriptor.Field(s => s.id).Type<NonNullType<IdType>>();
            descriptor.Field(s => s.name).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.surname).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.date_of_birth).Type<NonNullType<DateType>>();
            descriptor.Field(s => s.pesel).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.gender).Type<NonNullType<GenderType>>();

            descriptor.Field(s => s.address_id).Type<IdType>();
            descriptor.Field(s => s.account_id).Type<IdType>();

            // configure navigation propertiesd
            descriptor.Field(s => s.address)
                .Type<AddressType>()
                .ResolveWith<StudentResolvers>(r => r.GetAddress(default, default))
                .UseDbContext<UniversityContext>();
            descriptor.Field(s => s.account)
                .Type<AccountType>()
                .ResolveWith<StudentResolvers>(r => r.GetAccount(default, default))
                .UseDbContext<UniversityContext>();
        }

        private class StudentResolvers
        {
            public Students_Addresses GetAddress([Parent] Students student, [ScopedService] UniversityContext context)
            {
                return context.Addresses.FirstOrDefault(a => a.id == student.address_id);
            }

            public Users_Accounts GetAccount([Parent] Students student, [ScopedService] UniversityContext context)
            {
                return context.Accounts.FirstOrDefault(a => a.id == student.account_id);
            }
        }
    }
}
