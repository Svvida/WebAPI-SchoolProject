using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RazorPages.Pages.Admin.Roles
{
    [Authorize(Policy = "AdminPolicy")]
    public class DeleteModel : PageModel
    {
        private readonly IRoleService _roleService;

        public DeleteModel(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [BindProperty]
        public RoleDto Role { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Role = await _roleService.GetRoleByIdAsync(id);

            if (Role == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _roleService.DeleteRolesAsync(Role.Id);
            return RedirectToPage("./Index");
        }
    }
}
