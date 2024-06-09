using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RazorPages.Pages.Admin.Roles
{
    [Authorize(Policy = "EverybodyPolicy")]
    public class IndexModel : PageModel
    {
        private readonly IRoleService _roleService;

        public IndexModel(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IList<RoleDto> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = (await _roleService.GetAllRolesAsync()).ToList();
        }
    }
}
