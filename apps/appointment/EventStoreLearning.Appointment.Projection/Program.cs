using System;
using Autofac.Extensions.DependencyInjection;
using EventStoreLearning.Common.EventSourcing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace EventStoreLearning.Appointment.Projection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");

                var provider = CreateBuilder(args);

                var runner = provider.GetRequiredService<IProjectionProcessor>();
                runner.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IServiceProvider CreateBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .Build();

            var services = new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                });

            var factory = new AutofacServiceProviderFactory();

            var startup = new Startup(config);

            startup.ConfigureServices(services);
            var builder = factory.CreateBuilder(services);

            startup.ConfigureContainer(builder);
            var provider = factory.CreateServiceProvider(builder);

            return provider;
        }
    }
}
