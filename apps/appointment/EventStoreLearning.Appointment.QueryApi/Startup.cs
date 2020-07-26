using System;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using EventStoreLearning.Common.Utilities;
using EventStoreLearning.Common.Web.Extensions;
using ContextRunner;
using ContextRunner.NLog;
using EventStoreLearning.Mongo;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using EventStoreLearning.Common.Web;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(QueryApiConvention))]
[assembly: ApiConventionType(typeof(ErrorResponseApiConvention))]
namespace EventStoreLearning.Appointment.QueryApi
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
                Title = "Appointment Query API",
                Version = "v1",
                Description = "An API for querying appointment data."
            });

            services.Configure<MongoDbConfig>(Configuration.GetSection("AppointmentDB"));
            services.Configure<NlogContextRunnerConfig>(Configuration.GetSection("NlogContext"));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<NlogContextRunner>().As<IContextRunner>();

            builder.ConfigureMongo();

            builder.RegisterAssemblyTypes(_assemblies)
               .Where(t => t.Name.EndsWith("Repository", StringComparison.CurrentCulture))
               .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(_assemblies)
               .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCulture))
               .AsImplementedInterfaces();

            builder.RegisterType<MediatedDataContractFactory>().As<IMediatedDataContractFactory>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.ConfigureMvc(env, lifetime);
            app.ConfigureSwaggerUI("Appointment Query API");
        }
    }
}
