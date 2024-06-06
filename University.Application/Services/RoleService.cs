using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using University.Application.DTOs;
using University.Application.Interfaces;
using University.Domain.Entities;
using University.Domain.Interfaces;

namespace University.Application.Services
{
    public class RoleService : IRoleService
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
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> AddRolesAsync(CreateRoleDto createRoleDto)
        {
            var role = _mapper.Map<Roles>(createRoleDto);
            await _roleRepository.AddAsync(role);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task UpdateRolesAsync(RoleDto roleDto)
        {
            var existingRole = await _roleRepository.GetByIdAsync(roleDto.Id);
            if (existingRole == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            _mapper.Map(roleDto, existingRole);
            await _roleRepository.UpdateAsync(existingRole);
        }

        public async Task DeleteRolesAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }
            await _roleRepository.DeleteAsync(role.Id);
        }
    }
}
