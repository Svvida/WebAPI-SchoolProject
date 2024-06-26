﻿using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.Domain.Interfaces;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UniversityContext _context;

        public AccountRepository(UniversityContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users_Accounts>> GetAllAsync()
        {
            return await _context.Accounts
                .Include(a => a.Roles)
                    .ThenInclude(ur => ur.Role)
                    .ToListAsync();
        }
        public async Task<Users_Accounts> GetByIdAsync(Guid id)
        {
            return await _context.Accounts
                .Include(a => a.Roles)
                    .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Users_Accounts> AddAsync(Users_Accounts account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task UpdateAsync(Users_Accounts account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddRoleToAccountAsync(Guid userId, Guid roleId)
        {
            var account = await _context.Accounts.FindAsync(userId);
            var role = await _context.Roles.FindAsync(roleId);

            if (role is null || account is null)
            {
                throw new KeyNotFoundException("User or role doesn't exist");
            }

            var isRelationExist = await _context.UserAccountRoles
                .AnyAsync(uar => uar.RoleId == roleId && uar.AccountId == userId);
            if (isRelationExist)
            {
                throw new Exception("User already has this role");
            }

            var accountRole = new Users_Accounts_Roles
            {
                AccountId = userId,
                RoleId = roleId,
            };
            _context.UserAccountRoles.Add(accountRole);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleFromAccountAsync(Guid userId, Guid roleId)
        {
            var accountRole = await _context.UserAccountRoles
                .FirstOrDefaultAsync(uar => uar.AccountId == userId && uar.RoleId == roleId);

            if (accountRole is null)
            {
                throw new KeyNotFoundException("User doesn't have that role");
            }

            _context.UserAccountRoles.Remove(accountRole);
            await _context.SaveChangesAsync();
        }


        public async Task<Users_Accounts> GetByLoginAsync(string login)
        {
            if (login == null) throw new KeyNotFoundException("There is no student with that login");

            return await _context.Accounts.SingleOrDefaultAsync(a => a.Login == login);
        }

        public async Task<Users_Accounts> GetByLoginWithRolesAsync(string login)
        {
            return await _context.Accounts
                .Include(a => a.Roles)
                    .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(a => a.Login == login);
        }
    }
}
