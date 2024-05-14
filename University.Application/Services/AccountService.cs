using AutoMapper;
using Microsoft.AspNetCore.Identity;
using University.Application.DTOs;
using University.Domain.Entities;
using University.Domain.Interfaces;

namespace University.Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Users_Accounts?> _passwordHasher;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IPasswordHasher<Users_Accounts?> passwordHasher)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
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
            var accountEntity = await _accountRepository.GetByIdAsync(accountDto.Id);
            if (accountEntity is null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            _mapper.Map(accountDto, accountEntity);
            if(!string.IsNullOrWhiteSpace(accountDto.Password))
            {
                accountEntity.password = _passwordHasher.HashPassword(accountEntity, accountDto.Password);
            }

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
            // Hash password before saving it to the database
            accountEntity.password = _passwordHasher.HashPassword(accountEntity, accountDto.Password);
            await _accountRepository.AddAsync(accountEntity);
        }
    }
}
