using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using University.Domain.Entities;
using University.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace University.Tests.IntegrationTests
{
    public class IntegrationTestBase : IDisposable
    {
        protected readonly UniversityContext context;
        private readonly Mock<IConfiguration> mockConfiguration;

        public IntegrationTestBase()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new UniversityContext(options);

            // Initialize mock configuration
            mockConfiguration = new Mock<IConfiguration>();
            setUpConfiguration();

            var passwordHasher = new PasswordHasher<Users_Accounts?>();

            // Seed roles and accounts to the databsae
            UniversityContextSeed.Initialize(context, passwordHasher, mockConfiguration.Object);
        }

        private void setUpConfiguration()
        {
            mockConfiguration.SetupGet(config => config["SeedPasswords:Admin"]).Returns("MockAdminPassword");
            mockConfiguration.SetupGet(config => config["SeedPasswords:JanKowalski"]).Returns("MockJanKowalskiPassword");
            mockConfiguration.SetupGet(config => config["SeedPasswords:MartaRadzka"]).Returns("MockMartaRadzkaPassword");
            mockConfiguration.SetupGet(config => config["SeedPasswords:KamilNowak"]).Returns("MockKamilNowakPassword");
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
