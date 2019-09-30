using System;
using System.Linq;
using EventStoreLearning.Common.EventSourcing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventStoreLearning.EventStore;
using MediatR;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;
using MongoDB.Driver;
using EventStoreLearning.Common.Extensions;

namespace EventStoreLearning.Appointment.Projection
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var assembliesList = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
            assembliesList.Add(Assembly.GetExecutingAssembly());

            var assemblies = assembliesList.ToArray();

            services.AddMediatR(assemblies);

            var builder = new ContainerBuilder();

            builder.RegisterType<EventStoreClient>()
                .As<IEventStoreClient>()
                .WithParameter("connectionString", "ConnectTo=tcp://admin:changeit@localhost:1113; HeartBeatTimeout=500");

            builder.Register<IMongoDatabase>(context =>
            {
                var dbConnectionString = "mongodb://apptApp:apptPassword1@localhost/appointments";

                var client = new MongoClient(dbConnectionString);
                var database = client.GetDatabase("appointments");

                return database;
            });

            builder.RegisterType<AppointmentProjectionProcessor>()
                .As<IProjectionProcessor>();

            builder.RegisterType<EventMediator>()
                .As<IEventMediator>();

            builder.RegisterAssemblyTypes(assemblies)
               .Where(t => t.Name.EndsWith("Repository", StringComparison.CurrentCulture))
               .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
               .Where(t => t.Name.EndsWith("EventHandler", StringComparison.CurrentCulture))
               .AsSelf();

            var aggregateFactories = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(AggregateRoot).IsAssignableFrom(p) && p != typeof(AggregateRoot))
                .ToDictionary(
                t => t,
                t =>
                {
                    Func<AggregateRoot> factory = () => (AggregateRoot)Activator.CreateInstance(t);

                    return factory;
                });

            var eventDeserializers = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(Event).IsAssignableFrom(p) && p != typeof(Event))
                .ToDictionary(
                t => t.Name,
                t =>
                {
                    var deserializer = typeof(JsonEventDeserializer<>);
                    deserializer = deserializer.MakeGenericType(t);

                    var method = deserializer.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);

                    Func<string, Event> factory = (string json) => method.Invoke(null, new[] { json }).CastToReflected(t);

                    return factory;
                });

            builder.RegisterType<AggregateStore>()
               .As<IAggregateStore>()
               .WithParameter("aggregateFactories", aggregateFactories)
               .WithParameter("eventDeserializers", eventDeserializers);

            builder.Populate(services);

            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }
    }
}
