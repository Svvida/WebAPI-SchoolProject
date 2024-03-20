using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities;

namespace University.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Users_Accounts>> GetAllAsync();
        Task<Users_Accounts> GetByIdAsync(Guid id);
        Task<Users_Accounts> AddAsync(Users_Accounts account);
        Task UpdateAsync(Users_Accounts account);
        Task DeleteAsync(Guid id);
    }
}
