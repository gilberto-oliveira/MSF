using MSF.Common;
using MSF.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MSF.Domain.Factory
{
    public class DbContextFactory : IDesignTimeDbContextFactory<MSFDbContext>
    {
        public MSFDbContext CreateDbContext(string[] args)
        {
            var configuration = ConfigurationFactory.GetConfiguration();

            var builder = new DbContextOptionsBuilder<MSFDbContext>();

            var connection = configuration.GetConnectionString("MSF_DEV");

            builder.UseSqlServer(connection);

            return new MSFDbContext(builder.Options, null);
        }
    }
}
