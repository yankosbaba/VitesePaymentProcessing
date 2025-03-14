using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Vitese_Payment_Processing.Model;

namespace Vitese_Payment_Processing.Configuration
{
    public class AppSettingsFactory
    {
        private static readonly Lazy<IConfiguration> _configuration = new Lazy<IConfiguration>(BuildConfiguration);

        public static IConfiguration Configuration => _configuration.Value;

        private static IConfiguration BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public AppConfig LoadAppSettings()
        {
            var appSettings = new AppConfig();
            Configuration.Bind(appSettings);
            return appSettings;
        }
    }
}
