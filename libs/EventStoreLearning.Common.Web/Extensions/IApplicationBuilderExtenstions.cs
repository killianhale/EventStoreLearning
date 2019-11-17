using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EventStoreLearning.Common.Web.Extensions
{
    public static class IApplicationBuilderExtenstions
    {
        public static void ConfigureMvc(this IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
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
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
