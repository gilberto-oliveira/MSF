using MSF.Common;
using MSF.Identity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MSF.Identity.Factory
{
    public class DbContextFactory : IDesignTimeDbContextFactory<MSFIdentityDbContext>
    {
        public MSFIdentityDbContext CreateDbContext(string[] args)
        {
            var configuration = ConfigurationFactory.GetConfiguration();

            var builder = new DbContextOptionsBuilder<MSFIdentityDbContext>();

            var connection = configuration.GetConnectionString("MSF_DEV");

            builder.UseSqlServer(connection);

            return new MSFIdentityDbContext(builder.Options);
        }
    }
}
