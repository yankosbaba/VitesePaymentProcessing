using log4net;
using log4net.Config;
using log4net.Repository;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Vitese_Payment_Processing.Logging
{
    public static class LoggerFactory
    {
        private static readonly ServiceProvider _serviceProvider;

        static LoggerFactory()
        {
            var services = new ServiceCollection();

            // Configure log4net first
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            services.AddTransient<ILog>(provider =>
            {
                throw new InvalidOperationException("Resolve ILog via LoggerFactory.CreateLogger(Type)");
            });

            // Register ILogger as transient, using a factory that accepts a type
            services.AddTransient<ILogger>(provider =>
            {
                var type = provider.GetRequiredService<Type>();
                ILog log = LogManager.GetLogger(type);
                return new Log4NetAdapter(log);
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        public static ILogger CreateLogger(Type type)
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<ILogger>(_ =>
                {
                    ILog log = LogManager.GetLogger(type);
                    return new Log4NetAdapter(log);
                })
                .BuildServiceProvider();

            return serviceProvider.GetRequiredService<ILogger>();
        }

    }
}

