﻿using University.Domain.Entities;

namespace University.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Roles>> GetAllAsync();
        Task<Roles> GetByIdAsync(Guid id);
        Task AddAsync(Roles role);
        Task UpdateAsync(Roles role);
        Task DeleteAsync(Guid id);
    }
}
