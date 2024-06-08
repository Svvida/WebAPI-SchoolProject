using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.RazorPages.Services;

namespace University.RazorPages.Pages.Admin.Students
{
    [Authorize(Policy = "AdminPolicy")]
    public class IndexModel : PageModel
    {
        private readonly RestApiService _apiService;

        public IndexModel(RestApiService apiService)
        {
            _apiService = apiService;
        }

        public IList<StudentDto> Students { get; set; }

        public async Task OnGetAsync()
        {
            Students = await _apiService.GetStudentsAsync();
        }
    }
}
