using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace EventStoreLearning.Common.Web.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void ConfigureMvc(this IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.OutputFormatters.Clear();
            })
            .AddNewtonsoftJson(options =>
            {
                options.UseCamelCasing(false);
                options.AllowInputFormatterExceptionMessages = true;
                options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime;
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddHealthChecks();
        }

        public static void ConfigureSwagger(this IServiceCollection services, Assembly apiAssembly, string name, OpenApiInfo info)
        {
            services.AddSwaggerExamplesFromAssemblies(apiAssembly);

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(name, info);

                //config.ExampleFilters();

                var xmlFile = $"{apiAssembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath, true);
            });
        }
    }
}
