using AutoMapper;
using University.Application.DTOs;
using University.Domain.Entities;
using University.Domain.Interfaces;

namespace University.Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();

            return roles.Select(r => new RoleDto
            {
                Id = r.id,
                Name = r.name
            });
        }

        public async Task<RoleDto> AddRolesAsync(CreateRoleDto createRoleDto)
        {
            var role = _mapper.Map<Roles>(createRoleDto);
            await _roleRepository.AddAsync(role);

            return _mapper.Map<RoleDto>(role);
        }

        public async Task UpdateRolesAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Roles>(roleDto);
            await _roleRepository.UpdateAsync(role);
        }

        public async Task DeleteRolesAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Roles>(roleDto);
            await _roleRepository.DeleteAsync(role.id);
        }
    }
}
