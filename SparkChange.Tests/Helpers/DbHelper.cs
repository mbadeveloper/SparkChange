using Microsoft.EntityFrameworkCore;
using SparkChange.Domain;

namespace SparkChange.Tests.Helpers
{
    public static class DbHelper
    {
        public static DatabaseContext CreateDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase("SparkChange")
                .EnableSensitiveDataLogging(true)
                .Options;
            return new DatabaseContext(options);
        }
    }
}
