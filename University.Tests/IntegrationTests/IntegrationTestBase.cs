using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Infrastructure.Data;

namespace University.Tests.IntegrationTests
{
    public class IntegrationTestBase : IDisposable
    {
        protected readonly UniversityContext context;
        private readonly Mock<IConfiguration> configuration;

        public IntegrationTestBase()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new UniversityContext(options);

            configuration = new Mock<IConfiguration>();
        }

        private void setUpConfiguration()
        {

        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
