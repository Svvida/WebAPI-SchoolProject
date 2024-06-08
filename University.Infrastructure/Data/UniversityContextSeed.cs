using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using University.Domain.Entities;

namespace University.Infrastructure.Data
{
    public class UniversityContextSeed
    {
        private readonly IConfiguration _configuration;

        public UniversityContextSeed(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void Initialize(UniversityContext context, IPasswordHasher<Users_Accounts> passwordHasher, IConfiguration configuration)
        {
            var adminPassword = configuration["SeedPasswords:Admin"];
            var janKowalskiPassword = configuration["SeedPasswords:JanKowalski"];
            var martaRadzkaPassword = configuration["SeedPasswords:MartaRadzka"];
            var kamilNowakPassword = configuration["SeedPasswords:KamilNowak"];

            if (!context.Roles.Any() || !context.Accounts.Any())
            {
                var roles = new List<Roles>
        {
            new Roles { Id = SeedingConstants.AdminRoleId, Name = "Admin" },
            new Roles { Id = SeedingConstants.StudentRoleId, Name = "Student" },
            new Roles { Id = SeedingConstants.TeacherRoleId, Name = "Teacher" }
        };

                var usersAccounts = new List<Users_Accounts>
        {
            new Users_Accounts
            {
                Id = SeedingConstants.AdminAccountId,
                Email = "admin@wsei.pl",
                Login = "admin",
                Password = passwordHasher.HashPassword(null, adminPassword),
                IsActive = true,
            },
            new Users_Accounts
            {
                Id = new Guid("00000000-0000-0000-0000-000000000005"),
                Email = "jan.kowalski@wsei.pl",
                Login = "jan.kowalski",
                Password = passwordHasher.HashPassword(null, janKowalskiPassword),
                IsActive = true,
            },
            new Users_Accounts
            {
                Id = new Guid("00000000-0000-0000-0000-000000000006"),
                Email = "marta.radzka@wsei.pl",
                Login = "marta.radzka",
                Password = passwordHasher.HashPassword(null, martaRadzkaPassword),
                IsActive = true,
            },
            new Users_Accounts
            {
                Id = new Guid("00000000-0000-0000-0000-000000000007"),
                Email = "kamil.nowak@wsei.pl",
                Login = "kamil.nowak",
                Password = passwordHasher.HashPassword(null, kamilNowakPassword),
                IsActive = true,
            }
        };

                context.AddRange(usersAccounts);
                context.AddRange(roles);
                context.SaveChanges();
            }

            var adminAccount = context.Accounts.FirstOrDefault(a => a.Login == "admin");
            var janAccount = context.Accounts.FirstOrDefault(a => a.Login == "jan.kowalski");
            var martaAccount = context.Accounts.FirstOrDefault(a => a.Login == "marta.radzka");
            var kamilAccount = context.Accounts.FirstOrDefault(a => a.Login == "kamil.nowak");

            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
            var studentRole = context.Roles.FirstOrDefault(r => r.Name == "Student");
            var teacherRole = context.Roles.FirstOrDefault(r => r.Name == "Teacher");

            if (!context.UserAccountRoles.Any())
            {
                var userAccountRoles = new List<Users_Accounts_Roles>
        {
            new Users_Accounts_Roles { AccountId = adminAccount.Id, Account = adminAccount, RoleId = adminRole.Id, Role = adminRole },
            new Users_Accounts_Roles { AccountId = janAccount.Id, Account = janAccount, RoleId = studentRole.Id, Role = studentRole },
            new Users_Accounts_Roles { AccountId = martaAccount.Id, Account = martaAccount, RoleId = studentRole.Id, Role = studentRole },
            new Users_Accounts_Roles { AccountId = kamilAccount.Id, Account = kamilAccount, RoleId = teacherRole.Id, Role = teacherRole }
        };

                context.AddRange(userAccountRoles);
                context.SaveChanges();
            }

            if (!context.Students.Any())
            {
                var students = new List<Students>
        {
            new Students
            {
                Id = new Guid("10000000-0000-0000-0000-000000000001"),
                Name = "Jan",
                Surname = "Kowalski",
                DateOfBirth = DateTime.Parse("2002-12-08"),
                Pesel = "12345678901",
                Gender = Domain.Enums.Gender.Male,
                Address = new Students_Addresses
                {
                    Id = SeedingConstants.TestAddressId,
                    Country = "Poland",
                    City = "Kraków",
                    PostalCode = "48-345",
                    Street = "Motylowa",
                    BuildingNumber = "12",
                    ApartmentNumber = "10"
                },
                AccountId = janAccount.Id,
                Account = janAccount
            },
            new Students
            {
                Id = new Guid("10000000-0000-0000-0000-000000000002"),
                Name = "Marta",
                Surname = "Radzka",
                DateOfBirth = DateTime.Parse("2003-04-22"),
                Pesel = "98765432100",
                Gender = Domain.Enums.Gender.Female,
                Address = new Students_Addresses
                {
                    Id = new Guid("20000000-0000-0000-0000-000000000001"),
                    Country = "Poland",
                    City = "Olkusz",
                    PostalCode = "32-300",
                    Street = "Basztowa",
                    BuildingNumber = "12",
                    ApartmentNumber = "12"
                },
                AccountId = martaAccount.Id,
                Account = martaAccount
            }
        };

                context.AddRange(students);
                context.SaveChanges();
            }
        }

    }
}
