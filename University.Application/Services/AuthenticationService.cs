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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<Users_Accounts> _passwordHasher;

        public AuthenticationService(IAccountRepository accountRepository, IPasswordHasher<Users_Accounts> passwordHasher)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Users_Accounts> ValidateUserAsync(string login, string password)
        {
            // Fetch the user
            var user = await _accountRepository.GetByLoginAsync(login);
            if (user == null) throw new KeyNotFoundException("User not found");

            var result = _passwordHasher.VerifyHashedPassword(user, user.password, password);
            if(result == PasswordVerificationResult.Success)
            {
                return user;
            }

            throw new Exception("Password did not match");
        }
    }
}
