using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.RazorPages.Services;

namespace University.RazorPages.Pages.Admin.Students
{
    [Authorize(Policy = "AdminPolicy")]
    public class CreateModel : PageModel
    {
        private readonly RestApiService _apiService;

        public CreateModel(RestApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public StudentDto Student { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _apiService.CreateStudentAsync(Student);
            return RedirectToPage("./Index");
        }
    }
}
