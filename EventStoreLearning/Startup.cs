using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStoreLearning.Common.EventSourcing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EventStoreLearning.Common.Logging;
using EventStore.ClientAPI;
using EventStoreLearning.EventStore;
using MediatR;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventStoreLearning.Appointment;
using System.Reflection;

namespace EventStoreLearning
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMediatR(assemblies);

            var builder = new ContainerBuilder();

            builder.RegisterType<EventStoreClient>()
                .As<IEventStoreClient>()
                .WithParameter("connectionString", "ConnectTo=tcp://admin:changeit@localhost:1113; HeartBeatTimeout=500")
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandMediator>()
                .As<ICommandMediator>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
               .Where(t => t.Name.EndsWith("CommandHandler", StringComparison.CurrentCulture))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

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

            builder.RegisterType<AggregateStore>()
               .As<IAggregateStore>()
               .WithParameter("aggregateFactories", aggregateFactories)
               .InstancePerLifetimeScope();

            builder.Populate(services);

            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
