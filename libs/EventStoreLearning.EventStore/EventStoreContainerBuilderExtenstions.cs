using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using EventStoreLearning.Common.EventSourcing;
using EventStoreLearning.Common.Extensions;

namespace EventStoreLearning.EventStore
{
    public static class EventStoreContainerBuilderExtenstions
    {
        public static void ConfigureEventStore(this ContainerBuilder builder, Assembly[] assemblies, bool registerCommandHandlers = true, bool registerEventHandlers = true)
        {
            var eventDeserializers = new Dictionary<string, Func<string, Event>>();

            if(registerEventHandlers)
            {
                eventDeserializers = assemblies
                    .SelectMany(s => s.GetTypes())
                    .Where(p => typeof(Event).IsAssignableFrom(p) && p != typeof(Event))
                    .ToDictionary(
                    t => t.Name,
                    t =>
                    {
                        var deserializer = typeof(JsonEventDeserializer<>);
                        deserializer = deserializer.MakeGenericType(t);

                        var method = deserializer.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);

                        Event factory(string json) => method.Invoke(null, new[] { json }).CastToReflected(t);

                        return (Func<string, Event>)factory;
                    });
                
                builder.RegisterType<EventMediator>()
                   .As<IEventMediator>();

                builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.Name.EndsWith("EventHandler", StringComparison.CurrentCulture))
                   .AsSelf();
            }

            if(registerCommandHandlers)
            {
                builder.RegisterType<CommandMediator>()
                    .As<ICommandMediator>()
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.Name.EndsWith("CommandHandler", StringComparison.CurrentCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
            }

            builder.RegisterType<EventStoreClient>()
                .As<IEventStoreClient>();

            builder.RegisterType<EventRepository>()
               .As<IEventRepository>()
               .WithParameter("eventDeserializers", eventDeserializers);
        }
    }
}
