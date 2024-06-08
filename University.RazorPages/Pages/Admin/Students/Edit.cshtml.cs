using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.RazorPages.Services;

namespace University.RazorPages.Pages.Admin.Students
{
    [Authorize(Policy = "AdminPolicy")]
    public class EditModel : PageModel
    {
        private readonly RestApiService _apiService;

        public EditModel(RestApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public StudentDto Student { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Student = await _apiService.GetStudentByIdAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _apiService.UpdateStudentAsync(Student);
            return RedirectToPage("./Index");
        }
    }
}
