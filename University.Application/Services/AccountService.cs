using AutoMapper;
using University.Application.DTOs;
using University.Domain.Entities;
using University.Domain.Interfaces;

namespace University.Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();

            return accounts.Select(a => new AccountDto
            {
                Id = a.id,
                Email = a.email,
                Login = a.login,
                IsActive = a.is_active,
                DeactivationDate = a.deactivation_date
            });
        }

        public async Task<AccountDto> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account is null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            return _mapper.Map<AccountDto>(account);
        }

        public async Task UpdateAccountAsync(AccountDto accountDto)
        {
            // First we convert DTO to entity
            var accountEntity = _mapper.Map<Users_Accounts>(accountDto);
            // Then we can call a method to update it
            await _accountRepository.UpdateAsync(accountEntity);
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var accountToDelete = await _accountRepository.GetByIdAsync(id);
            if (accountToDelete is null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            await _accountRepository.DeleteAsync(id);
        }

        public async Task AddAccountAsync(AccountDto accountDto)
        {
            var accountEntity = _mapper.Map<Users_Accounts>(accountDto);
            await _accountRepository.AddAsync(accountEntity);
        }
    }
}
