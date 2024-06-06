using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using University.RazorPages.Services;

namespace University.RazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly RestApiService _restApiService;

        public LoginModel(RestApiService restApiService)
        {
            _restApiService = restApiService;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = await _restApiService.LoginAsync(Username, Password);
            if (string.IsNullOrEmpty(token))
            {
                ErrorMessage = "Invalid login attempt.";
                return Page();
            }

            // Save the token in the session or cookie
            HttpContext.Session.SetString("JWToken", token);

            return RedirectToPage("/Index");
        }
    }
}
