using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using EventStoreLearning.EventSourcing;
using EventStoreLearning.EventSourcing.EventStore;
using EventStoreLearning.EventSourcing.EventStore.Deserializers;
using EventStoreLearning.EventSourcing.EventStore.Extensions;

namespace EventStoreLearning.DependencyInjection.EventStore
{
    public static class EventStoreContainerBuilderExtenstions
    {
        public static void ConfigureEventStore(this ContainerBuilder builder, Assembly[] assemblies, bool registerCommandHandlers = true, bool registerEventHandlers = true)
        {
            var eventDeserializers = new Dictionary<string, Func<string, IEvent>>();

            if(registerEventHandlers)
            {
                eventDeserializers = assemblies
                    .SelectMany(s => s.GetTypes())
                    .Where(p => typeof(IEvent).IsAssignableFrom(p) && p != typeof(IEvent))
                    .ToDictionary(
                    t => t.Name,
                    t =>
                    {
                        var deserializer = typeof(JsonEventDeserializer<>);
                        deserializer = deserializer.MakeGenericType(t);

                        var method = deserializer.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);

                        IEvent factory(string json) => method.Invoke(null, new[] { json }).CastToReflected(t);

                        return (Func<string, IEvent>)factory;
                    });

                builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.Name.EndsWith("EventHandler", StringComparison.CurrentCulture))
                   .AsSelf();
            }

            if(registerCommandHandlers)
            {
                builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.Name.EndsWith("CommandHandler", StringComparison.CurrentCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
            }

            builder.RegisterType<AggregateOrchestrator>()
                .As<IAggregateOrchestrator>();

            builder.RegisterType<EventStoreClient>()
                .As<IEventStoreClient>();

            builder.RegisterType<EventRepository>()
               .As<IEventRepository>()
               .WithParameter("eventDeserializers", eventDeserializers);
        }
    }
}
