using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using University.Application.Mappers;
using University.Domain.Entities;
using University.Domain.Interfaces;
using University.Infrastructure.Data;
using University.Infrastructure.Repositories;

namespace University.RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load secrets.json for development purposes
            builder.Configuration.AddJsonFile("secrets.json", optional:true, reloadOnChange: true);

            // Configure JWT authentication service
            var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"];

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
                    };
                });

            // Configure database context
            builder.Services.AddDbContext<UniversityContext>(options =>
                options.UseInMemoryDatabase("University"));

            // Register password hasher for user accounts
            builder.Services.AddScoped<IPasswordHasher<Users_Accounts>, PasswordHasher<Users_Accounts>>();

            // Repositories
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            // Automapper configuration
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Controllers
            builder.Services.AddControllers();

            // Swagger configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var services =scope.ServiceProvider;
                var context = services.GetRequiredService<UniversityContext>();
                var passwordHasher = services.GetRequiredService<IPasswordHasher<Users_Accounts?>>();
                var configuration = services.GetRequiredService<IConfiguration>();
                UniversityContextSeed.Initialize(context, passwordHasher, configuration);
            }

                app.Run();
        }
    }
}
