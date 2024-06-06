using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities;
using University.Domain.Interfaces;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly UniversityContext _context;

        public AddressRepository(UniversityContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Students_Addresses>> GetAllAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<Students_Addresses> GetByIdAsync(Guid id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if(address is null) 
                throw new KeyNotFoundException("Address not found");
            return address;
        }

        public async Task<Students_Addresses> CreateAsync(Students_Addresses address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Students_Addresses> UpdateAsync(Students_Addresses address)
        {
            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task DeleteAsync(Guid id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address is null)
                throw new KeyNotFoundException("Address not found");
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }
}
