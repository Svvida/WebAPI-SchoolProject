using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using University.Application.Interfaces;
using University.Application.Services;
using University.Domain.Entities;
using University.Domain.Interfaces;
using University.Infrastructure.Data;
using University.Infrastructure.Repositories;
using University.RazorPages.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add configuration
var configuration = builder.Configuration;

builder.Services.AddDbContext<UniversityContext>(options =>
    options.UseInMemoryDatabase("University"));

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust the session timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient<RestApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7084");
});

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add logging
builder.Services.AddLogging();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Repositories and Services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddSingleton<IPasswordHasher<Users_Accounts?>, PasswordHasher<Users_Accounts?>>();

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("StudentPolicy", policy => policy.RequireRole("Student"));
    options.AddPolicy("TeacherPolicy", policy => policy.RequireRole("Teacher"));
    options.AddPolicy("AdminOrTeacherPolicy", policy => policy.RequireRole("Admin", "Teacher"));
    options.AddPolicy("AdminOrStudentPolicy", policy => policy.RequireRole("Admin", "Student"));
    options.AddPolicy("EveryonePolicy", policy => policy.RequireRole("Admin", "Student", "Teacher"));
});

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UniversityContext>();
    var passwordHasher = services.GetRequiredService<IPasswordHasher<Users_Accounts>>();
    var configurationDb = services.GetRequiredService<IConfiguration>();
    UniversityContextSeed.Initialize(context, passwordHasher, configurationDb);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
