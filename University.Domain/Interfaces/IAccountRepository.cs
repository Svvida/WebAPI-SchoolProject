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
        Task AddRoleToAccountAsync(Guid userId, Guid roleId);
        Task DeleteRoleFromAccountAsync(Guid userId, Guid roleId);
    }
}
