using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using EventStoreLearning.Common.EventSourcing;
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
using EventStoreLearning.EventStore;
using Microsoft.Extensions.Options;

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
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureEventStore(_assemblies, true, false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.ConfigureMvc(env, lifetime);
            app.ConfigureSwaggerUI("Appointment Command API");
        }
    }
}
