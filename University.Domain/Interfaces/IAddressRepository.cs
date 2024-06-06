using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities;

namespace University.Domain.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Students_Addresses>> GetAllAsync();
        Task<Students_Addresses> GetByIdAsync(Guid id);
        Task<Students_Addresses> CreateAsync(Students_Addresses address);
        Task<Students_Addresses> UpdateAsync(Students_Addresses address);
        Task DeleteAsync(Guid id);
    }
}
