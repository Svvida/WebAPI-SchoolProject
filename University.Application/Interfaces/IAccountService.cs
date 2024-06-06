using University.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Domain.Entities;

namespace University.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task<AccountDto> GetAccountByIdAsync(Guid id);
        Task UpdateAccountAsync(AccountDto accountDto);
        Task DeleteAccountAsync(Guid id);
        Task AddAccountAsync(AccountDto accountDto);
        Task<Users_Accounts> ValidateUserAsync(string login, string password);
    }
}
