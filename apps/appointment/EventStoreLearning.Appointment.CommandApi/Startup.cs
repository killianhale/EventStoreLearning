using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using EventStoreLearning.EventSourcing;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DocExpansion = Swashbuckle.AspNetCore.SwaggerUI.DocExpansion;
using Microsoft.OpenApi.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc.Formatters;
using EventStoreLearning.Common.Web.Extensions;
using EventStoreLearning.Common.Utilities;
using EventStoreLearning.EventSourcing.EventStore;
using ActionContext = ContextRunner.Base.ActionContext;
using Microsoft.Extensions.Options;
using NLog;
using EventStoreLearning.Common.Web.Middleware;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using EventStoreLearning.DependencyInjection.EventStore;
using ContextRunner;
using ContextRunner.NLog;
using ContextRunner.State;
using ContextRunner.FlatIO;

namespace EventStoreLearning.Appointment.CommandApi
{
    internal class Startup
    {
        private readonly Assembly[] _assemblies;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _assemblies = AssemblyHelper
                .GetAllOriginalAssembliesAroundType(typeof(Startup))
                .AddAssemblyFromType(typeof(Exception));
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(_assemblies);

            services.ConfigureMvc();
            services.ConfigureSwagger(Assembly.GetExecutingAssembly(), "v1", new OpenApiInfo
            {
                Title = "Appointment Command API",
                Version = "v1",
                Description = "An API for publishing commands for manipulating appointment data."
            });

            services.Configure<EventStoreConfig>(Configuration.GetSection("EventStore"));
            services.Configure<NlogContextRunnerConfig>(Configuration.GetSection("NlogContext"));
            services.Configure<FlatIOContextRunnerConfig>(Configuration.GetSection("FlatContext"));

            ActionContext.Loaded += Context_Loaded;
            ActionContext.Unloaded += Context_Unloaded;
        }

        private void Context_Loaded(ActionContext context)
        {
            var logger = LogManager.GetLogger("Metrics");
            logger.Info($"New context '{context.ContextName}' loaded");
        }

        private void Context_Unloaded(ActionContext context)
        {
            var logger = LogManager.GetLogger("Metrics");
            logger.Info($"Context '{context.ContextName}' unloaded with {context.TimeElapsed.Milliseconds} MS total runtime.");
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<NlogContextRunner>().As<IContextRunner>();
            //builder.RegisterType<FlatIOContextRunner>().As<IContextRunner>();

            builder.ConfigureEventStore(_assemblies, true, true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.ConfigureMvc(env, lifetime);
            app.ConfigureSwaggerUI("Appointment Command API");
        }
    }
}
