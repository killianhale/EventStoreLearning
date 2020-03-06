using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace EventStoreLearning.Common.Web.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void ConfigureMvc(this IServiceCollection services)
        {
            services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.AllowInputFormatterExceptionMessages = true;
                options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime;
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatString = "YYYY-MM-DDTHH:mm:ss.sssZ";
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services, Assembly apiAssembly, string name, OpenApiInfo info)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(name, info);

                var xmlFile = $"{apiAssembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath, true);
            });
        }
    }
}
