using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using EventStoreLearning.Appointment.ReadModel;
using EventStoreLearning.Common.Querying;
using EventStoreLearning.Common.Utilities;
using EventStoreLearning.Common.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

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
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register<IMongoDatabase>(context =>
            {
                var dbConnectionString = "mongodb://apptApp:apptPassword1@localhost/appointments";

                var client = new MongoClient(dbConnectionString);
                var database = client.GetDatabase("appointments");

                return database;
            });

            builder.RegisterType<AppointmentQueryMediator>()
                .As<IQuery>();

            builder.RegisterAssemblyTypes(_assemblies)
               .Where(t => t.Name.EndsWith("Repository", StringComparison.CurrentCulture))
               .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(_assemblies)
               .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCulture))
               .AsImplementedInterfaces();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.ConfigureMvc(env, lifetime);
            app.ConfigureSwaggerUI("Appointment Query API");
        }
    }
}
