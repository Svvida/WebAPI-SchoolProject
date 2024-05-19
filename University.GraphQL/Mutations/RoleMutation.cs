using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Mutations
{
    public class RoleInput
    {
        public string Name { get; set; }
    }

    public class RoleMutation
    {
        public async Task<Roles> AddRoleAsync(RoleInput input, [Service] UniversityContext context)
        {
            var role = new Roles
            {
                Id = Guid.NewGuid(),
                Name = input.Name
            };

            context.Roles.Add(role);
            await context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRoleAsync(Guid id, [Service] UniversityContext context)
        {
            var role = await context.Roles.FindAsync(id);
            if (role == null) return false;

            context.Roles.Remove(role);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Roles> UpdateRoleAsync(Guid id, string name, [Service] UniversityContext context)
        {
            var role = await context.Roles.FindAsync(id);
            if (role == null) throw new Exception("Role not found");

            role.Name = name;
            await context.SaveChangesAsync();
            return role;
        }
    }
}
