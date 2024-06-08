using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RazorPages.Pages.Admin.Students
{
    [Authorize(Policy = "AdminPolicy")]
    public class DetailsModel : PageModel
    {
        private readonly IStudentService _studentService;

        public DetailsModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public StudentDto Student { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Student = await _studentService.GetStudentByIdAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
