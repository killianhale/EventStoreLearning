using System;
using EventStoreLearning.Common.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using ContextRunner.Http;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace EventStoreLearning.Common.Web.Extensions
{
    public static class IApplicationBuilderExtenstions
    {
        public static void ConfigureMvc(this IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.UseRouting();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseContextRunnerHttpMiddleware();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => false
                });

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = check => check.Tags.Contains("ready")
                });
            });
        }

        public static void ConfigureSwaggerUI(this IApplicationBuilder app, string name)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.DocExpansion(DocExpansion.List);
                config.EnableDeepLinking();

                config.RoutePrefix = string.Empty;
                config.SwaggerEndpoint("/swagger/v1/swagger.json", name);
            });
        }
    }
}
