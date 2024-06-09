using AutoMapper;
using Microsoft.AspNetCore.Identity;
using University.Application.DTOs;
using University.Application.Interfaces;
using University.Domain.Entities;
using University.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace University.Application.Services
{
    public class AccountService : IAccountService
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
                Id = a.Id,
                Email = a.Email,
                Login = a.Login,
                IsActive = a.IsActive,
                DeactivationDate = a.DeactivationDate,
                Roles = a.Roles.Select(r => new RoleDto
                {
                    Id = r.Role.Id,
                    Name = r.Role.Name
                }).ToList()
            });
        }

        public async Task<AccountDto> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            return _mapper.Map<AccountDto>(account);
        }

        public async Task UpdateAccountAsync(AccountDto accountDto)
        {
            var accountEntity = await _accountRepository.GetByIdAsync(accountDto.Id);
            if (accountEntity == null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            _mapper.Map(accountDto, accountEntity);
            if (!string.IsNullOrWhiteSpace(accountDto.Password))
            {
                accountEntity.Password = _passwordHasher.HashPassword(accountEntity, accountDto.Password);
            }

            await _accountRepository.UpdateAsync(accountEntity);
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var accountToDelete = await _accountRepository.GetByIdAsync(id);
            if (accountToDelete == null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            await _accountRepository.DeleteAsync(id);
        }

        public async Task AddAccountAsync(AccountDto accountDto)
        {
            var accountEntity = _mapper.Map<Users_Accounts>(accountDto);
            // Hash password before saving it to the database
            accountEntity.Password = _passwordHasher.HashPassword(accountEntity, accountDto.Password);
            await _accountRepository.AddAsync(accountEntity);
        }

        public async Task<Users_Accounts> ValidateUserAsync(string login, string password)
        {
            // Fetch the user including roles
            var user = await _accountRepository.GetByLoginWithRolesAsync(login);

            if (user != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (result == PasswordVerificationResult.Success)
                {
                    return user;
                }
            }

            return null;
        }

        public async Task<IEnumerable<RoleDto>> GetAccountRolesAsync(Guid accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            return account.Roles.Select(r => new RoleDto
            {
                Id = r.Role.Id,
                Name = r.Role.Name
            });
        }
    }
}
