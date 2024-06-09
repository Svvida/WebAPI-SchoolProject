using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using University.RazorPages.Services;

namespace University.RazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly RestApiService _restApiService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(RestApiService restApiService, ILogger<LoginModel> logger)
        {
            _restApiService = restApiService;
            _logger = logger;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var token = await _restApiService.LoginAsync(Username, Password);
                _logger.LogInformation("Token received: {Token}", token);

                if (string.IsNullOrEmpty(token))
                {
                    ErrorMessage = "Invalid login attempt.";
                    return Page();
                }

                // Extract roles and claims from the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, jwtToken.Claims.First(c => c.Type == "nameid").Value),
            new Claim(ClaimTypes.Name, Username),
            new Claim("JWT", token)
        };

                // Adjust role claim extraction
                claims.AddRange(jwtToken.Claims.Where(c => c.Type == "role").Select(c => new Claim(ClaimTypes.Role, c.Value)));

                // Log all claims
                _logger.LogInformation("Claims: {Claims}", string.Join(", ", claims.Select(c => $"{c.Type}: {c.Value}")));

                // Log roles specifically
                var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                _logger.LogInformation("Roles: {Roles}", string.Join(", ", roles));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                _logger.LogError(ex, "Login failed");
                return Page();
            }
        }


    }
}
