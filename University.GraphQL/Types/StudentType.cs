using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Types
{
    public class StudentType : ObjectType<Students>
    {
        protected override void Configure(IObjectTypeDescriptor<Students> descriptor)
        {
            descriptor.Field(s => s.Id).Type<NonNullType<IdType>>();
            descriptor.Field(s => s.Name).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.Surname).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.DateOfBirth).Type<NonNullType<DateType>>();
            descriptor.Field(s => s.Pesel).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.Gender).Type<NonNullType<GenderType>>();

            descriptor.Field(s => s.AddressId).Type<IdType>();
            descriptor.Field(s => s.AccountId).Type<IdType>();

            descriptor.Field(s => s.Address)
                .Type<AddressType>()
                .ResolveWith<StudentResolvers>(r => r.GetAddress(default, default))
                .UseDbContext<UniversityContext>()
                .Name("address");

            descriptor.Field(s => s.Account)
                .Type<AccountType>()
                .ResolveWith<StudentResolvers>(r => r.GetAccount(default, default))
                .UseDbContext<UniversityContext>()
                .Name("account");
        }

        private class StudentResolvers
        {
            public async Task<Students_Addresses> GetAddress([Parent] Students student, [Service] IDbContextFactory<UniversityContext> dbContextFactory)
            {
                using var context = dbContextFactory.CreateDbContext();
                return await context.Addresses
                    .FirstOrDefaultAsync(a => a.Id == student.AddressId);
            }

            public async Task<Users_Accounts> GetAccount([Parent] Students student, [Service] IDbContextFactory<UniversityContext> dbContextFactory)
            {
                using var context = dbContextFactory.CreateDbContext();
                return await context.Accounts
                    .Include(a => a.Roles)
                    .ThenInclude(uar => uar.Role)
                    .FirstOrDefaultAsync(a => a.Id == student.AccountId);
            }
        }
    }
}
