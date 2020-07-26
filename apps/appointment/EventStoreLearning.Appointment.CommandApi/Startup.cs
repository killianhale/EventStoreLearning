using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
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
using ActionContext = ContextRunner.Base.ActionContext;
using Microsoft.Extensions.Options;
using NLog;
using EventStoreLearning.Common.Web.Middleware;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using ContextRunner;
using ContextRunner.NLog;
using ContextRunner.State;
using EventStoreLearning.Common.Web;
using EventStoreLearning.Appointment.CommandApi;
using Swashbuckle.AspNetCore.Filters;
using EventStoreLearning.Appointment.CommandApi.Contract.Examples;
using AggregateOP.EventStore;
using AggregateOP;
using AggregateOP.MediatR;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(CommandApiConvention))]
[assembly: ApiConventionType(typeof(ErrorResponseApiConvention))]
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
            services.Configure<EventStoreConfig>(Configuration.GetSection("EventStore"));
            services.Configure<NlogContextRunnerConfig>(Configuration.GetSection("NlogContext"));

            services.AddAutoMapper(typeof(Startup));

            services.ConfigureMvc();
            services.ConfigureSwagger(Assembly.GetExecutingAssembly(), "v1", new OpenApiInfo
            {
                Title = "Appointment Command API",
                Version = "v1",
                Description = "An API for publishing commands for manipulating appointment data."
            });

            services.ConfigureAggregateOP(_assemblies, factory =>
            {
                factory.AddMediatR();
                factory.AddEventStore();

                factory.AddCommandHandlers();
                factory.AddEventHandlers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<NlogContextRunner>().As<IContextRunner>();

            builder.RegisterType<MediatedDataContractFactory>().As<IMediatedDataContractFactory>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.ConfigureMvc(env, lifetime);
            app.ConfigureSwaggerUI("Appointment Command API");
        }
    }
}
