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

        public static void Initialize(UniversityContext context, IPasswordHasher<Users_Accounts?> passwordHasher, IConfiguration configuration)
        {
            var adminPassword = configuration["SeedPasswords:Admin"];
            var janKowalskiPassword = configuration["SeedPasswords:JanKowalski"];
            var martaRadzkaPassword = configuration["SeedPasswords:MartaRadzka"];
            var kamilNowakPassword = configuration["SeedPasswords:KamilNowak"];

            // Seed roles and accounts to the databsae 
            if (!context.Roles.Any() || !context.Accounts.Any())
            {
                var roles = new List<Roles>
                {
                    new Roles
                    {
                        id = SeedingConstants.AdminRoleId,
                        name = "Admin"
                    },
                    new Roles
                    {
                        id = SeedingConstants.StudentRoleId,
                        name = "Student"
                    },
                    new Roles
                    {
                        id = SeedingConstants.TeacherRoleId,
                        name = "Teacher"
                    }
                };


                var usersAccounts = new List<Users_Accounts>
                {
                    new Users_Accounts
                    {
                        id = SeedingConstants.AdminAccountId,
                        email = "admin@wsei.pl",
                        login = "admin",
                        password = passwordHasher.HashPassword(null, adminPassword),
                        is_active = true,
                    },
                    new Users_Accounts
                    {
                        id = Guid.NewGuid(),
                        email = "jan.kowalski@wsei.pl",
                        login = "jan.kowalski",
                        password = passwordHasher.HashPassword(null,janKowalskiPassword),
                        is_active = true,
                    },
                    new Users_Accounts
                    {
                        id = Guid.NewGuid(),
                        email = "marta.radzka@wsei.pl",
                        login = "marta.radzka",
                        password = passwordHasher.HashPassword(null, martaRadzkaPassword),
                        is_active = true,
                    },
                    new Users_Accounts
                    {
                        id = Guid.NewGuid(),
                        email = "kamil.nowak@wsei.pl",
                        login = "kamil.nowak",
                        password = passwordHasher.HashPassword(null,kamilNowakPassword),
                        is_active = true,
                    }
                };
                context.AddRange(usersAccounts);
                context.AddRange(roles);
                context.SaveChanges();
            }

            var adminAccount = context.Accounts.FirstOrDefault(a => a.login == "admin");
            var janAccount = context.Accounts.FirstOrDefault(a => a.login == "jan.kowalski");
            var martaAccount = context.Accounts.FirstOrDefault(a => a.login == "marta.radzka");
            var kamilAccount = context.Accounts.FirstOrDefault(a => a.login == "kamil.nowak");

            var adminRole = context.Roles.FirstOrDefault(r => r.name == "Admin");
            var studentRole = context.Roles.FirstOrDefault(r => r.name == "Student");
            var teacherRole = context.Roles.FirstOrDefault(r => r.name == "Teacher");

            // Check if there are any accounts with roles in the database
            if (!context.UserAccountRoles.Any())
            {
                var userAccountRoles = new List<Users_Accounts_Roles>
                {
                    new Users_Accounts_Roles
                    {
                        account_id = adminAccount.id,
                        account = adminAccount,
                        role_id = adminRole.id,
                        role = adminRole
                    },
                    new Users_Accounts_Roles
                    {
                        account_id = janAccount.id,
                        account = janAccount,
                        role_id= studentRole.id,
                        role = studentRole
                    },
                    new Users_Accounts_Roles
                    {
                        account_id = martaAccount.id,
                        account = martaAccount,
                        role_id = studentRole.id,
                        role = studentRole
                    },
                    new Users_Accounts_Roles
                    {
                        account_id = kamilAccount.id,
                        account = kamilAccount,
                        role_id = teacherRole.id,
                        role = teacherRole
                    }
                };

                context.AddRange(userAccountRoles);
                context.SaveChanges();
            }

            // check if there is any student in database
            if (!context.Students.Any())
            {
                var students = new List<Students>
                {
                    new Students
                    {
                        id = Guid.NewGuid(),
                        name = "Jan",
                        surname = "Kowalski",
                        date_of_birth = DateTime.Parse("2002-12-08"),
                        pesel = "12345678901",
                        gender = Domain.Enums.Gender.Male,
                        address = new Students_Addresses
                        {
                            country = "Poland",
                            city = "Kraków",
                            postal_code = "48-345",
                            street = "Motylowa",
                            building_number = "12",
                            apartment_number = "10"
                        },
                        account_id = janAccount.id,
                        account = janAccount
                    },
                    new Students
                    {
                        id = Guid.NewGuid() ,
                        name = "Marta",
                        surname = "Radzka",
                        date_of_birth = DateTime.Parse("2003-04-22"),
                        pesel = "98765432100",
                        gender = Domain.Enums.Gender.Female,
                        address = new Students_Addresses
                        {
                            country = "Poland",
                            city = "Olkusz",
                            postal_code = "32-300",
                            street = "Basztowa",
                            building_number = "12",
                            apartment_number = "12"
                        },
                        account_id = martaAccount.id,
                        account = martaAccount
                    }
                };

                context.AddRange(students);
                context.SaveChanges();
            };
        }
    }
}
