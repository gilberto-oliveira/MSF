using Microsoft.Extensions.Configuration;

namespace MSF.Common
{
    public class ConfigurationFactory
    {
        public static IConfigurationRoot GetConfiguration() => new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
    }
}
