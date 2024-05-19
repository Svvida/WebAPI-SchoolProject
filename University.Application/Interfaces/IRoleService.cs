using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.DTOs;

namespace University.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(Guid id);
        Task<RoleDto> AddRolesAsync(CreateRoleDto createRoleDto);
        Task UpdateRolesAsync(RoleDto roleDto);
        Task DeleteRolesAsync(Guid id);
    }

}
