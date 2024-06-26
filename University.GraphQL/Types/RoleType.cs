﻿using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Types
{
    public class RoleType : ObjectType<Roles>
    {
        protected override void Configure(IObjectTypeDescriptor<Roles> descriptor)
        {
            descriptor.Field(r => r.Id).Type<NonNullType<IdType>>();
            descriptor.Field(r => r.Name).Type<NonNullType<StringType>>();
            descriptor.Field(r => r.Accounts)
                .Type<ListType<AccountRoleType>>()
                .ResolveWith<RoleResolver>(r => r.GetUar(default!, default!))
                .UseDbContext<UniversityContext>();
        }

        private class RoleResolver
        {
            public IQueryable<Users_Accounts_Roles> GetUar([Parent] Roles role, [Service] IDbContextFactory<UniversityContext> dbContextFactory)
            {
                using var context = dbContextFactory.CreateDbContext();
                return context.UserAccountRoles.Where(uar => uar.RoleId == role.Id);
            }
        }
    }
}
