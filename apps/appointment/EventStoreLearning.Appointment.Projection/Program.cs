using System;
using AggregateOP;
using Autofac.Extensions.DependencyInjection;
using ContextRunner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ReasonCodeExceptions;
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
                var reasonCode = ex is ReasonCodeException reasonEx ? reasonEx.ReasonCode : "?";
                var message = ex.Message;

                logger.Error($"Stopped program because of exception!\n\n{reasonCode}: {message}");

                Environment.Exit(1);
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
