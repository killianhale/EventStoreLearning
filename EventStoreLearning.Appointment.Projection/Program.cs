using System;
using System.Threading.Tasks;
using EventStoreLearning.Common.EventSourcing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace EventStoreLearning.Appointment.Projection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");

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
                    
                var startup = new Startup(config);
                var servicesProvider = startup.ConfigureServices(services);

                using (servicesProvider as IDisposable)
                {
                    var runner = servicesProvider.GetRequiredService<IProjectionProcessor>();
                    runner.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
    }
}
