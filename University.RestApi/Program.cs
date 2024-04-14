using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using University.Application.Mappers;
using University.Application.NewFolder;
using University.Application.Services;
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
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Configure database context
            builder.Services.AddDbContext<UniversityContext>(options =>
                options.UseInMemoryDatabase("University"));

            //Register Authentication service
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

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
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat="JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using Bearer scheme"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id= "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

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
