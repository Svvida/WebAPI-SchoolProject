using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities;
using University.Domain.Interfaces;

namespace University.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<Users_Accounts> _passwordHasher;

        public AuthService(IAccountRepository accountRepository, IPasswordHasher<Users_Accounts> passwordHasher)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Users_Accounts> ValidateUserAsync(string login, string password)
        {
            // Fetch the user
            var user = await _accountRepository.GetByLoginAsync(login);

            if (user is not null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.password, password);
                if(result == PasswordVerificationResult.Success)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
