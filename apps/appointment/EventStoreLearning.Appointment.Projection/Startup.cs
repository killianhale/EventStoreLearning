using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using EventStoreLearning.Appointment.ReadModel;
using EventStoreLearning.Common.Utilities;
using EventStoreLearning.Mongo;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ContextRunner.NLog;
using ContextRunner;
using AggregateOP.EventStore;
using AggregateOP;
using AggregateOP.MediatR;

namespace EventStoreLearning.Appointment.Projection
{
    public class Startup
    {
        private readonly Assembly[] _assemblies;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _assemblies = AssemblyHelper.GetAllOriginalAssembliesAroundType(typeof(Startup));
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EventStoreConfig>(Configuration.GetSection("EventStore"));
            services.Configure<MongoDbConfig>("appointment-db", Configuration.GetSection("AppointmentDB"));
            services.Configure<NlogContextRunnerConfig>(Configuration.GetSection("NlogContext"));

            services.ConfigureAggregateOP(_assemblies, factory =>
            {
                factory.AddMediatR();
                factory.AddEventStore();

                factory.AddEventHandlers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<NlogContextRunner>().As<IContextRunner>();

            builder.ConfigureMongo("appointment-db");

            builder.RegisterType<AppointmentProjectionProcessor>()
                .As<IProjectionProcessor>();

            builder.RegisterAssemblyTypes(_assemblies)
               .Where(t => t.Name.EndsWith("Repository", StringComparison.CurrentCulture) && t != typeof(EventRepository))
               .AsImplementedInterfaces();
        }
    }
}
