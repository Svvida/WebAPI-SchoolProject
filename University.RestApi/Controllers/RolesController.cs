using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> Get(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role is null)
                return NotFound("Role not found");

            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create(CreateRoleDto createRoleDto)
        {
            var role = await _roleService.AddRolesAsync(createRoleDto);
            return CreatedAtAction(nameof(Get), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, RoleDto roleDto)
        {
            if (id != roleDto.Id)
                return BadRequest();

            var existingRole = await _roleService.GetRoleByIdAsync(id);
            if (existingRole is null)
                return NotFound();

            await _roleService.UpdateRolesAsync(roleDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingRole = await _roleService.GetRoleByIdAsync(id);
            if (existingRole is null)
                return NotFound();

            await _roleService.DeleteRolesAsync(id);
            return NoContent();
        }
    }
}
