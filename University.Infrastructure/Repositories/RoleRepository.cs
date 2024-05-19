using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Domain.Entities;
using University.Domain.Interfaces;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UniversityContext _context;

        public RoleRepository(UniversityContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Roles>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Roles> GetByIdAsync(Guid id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task AddAsync(Roles role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Roles role)
        {
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
