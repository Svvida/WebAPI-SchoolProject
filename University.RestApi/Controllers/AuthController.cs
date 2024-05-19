using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using University.Application.DTOs;
using University.Application.Services;
using University.Domain.Entities;
using University.Domain.Interfaces;

namespace University.RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<Users_Accounts> _passwordHasher;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IPasswordHasher<Users_Accounts> passwordHasher, IAuthService authenticationService)
        {
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _authService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _authService.ValidateUserAsync(loginDto.Login, loginDto.Password);
                if (user is not null)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { token });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid login or password", ex);
            }

            // Always return the same error message for extended security
            return Unauthorized("Invalid login attemp.");
        }

        private string GenerateJwtToken(Users_Accounts user)
        {
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
