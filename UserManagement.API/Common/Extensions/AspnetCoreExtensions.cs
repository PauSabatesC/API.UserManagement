using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserManagement.API.Controllers.HealthChecks;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Enums;
using UserManagement.Infrastructure.Database;
using UserManagement.Services.Options;

namespace UserManagement.API.Extensions
{
    public static class AspnetCoreExtensions
    {
        public static async Task<IHost> CreateDatabase<T>(this IHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    await db.Database.MigrateAsync();

                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    if(!await roleManager.RoleExistsAsync(Roles.Admin))
                    {
                        var adminRole = new IdentityRole(Roles.Admin);
                        await roleManager.CreateAsync(adminRole);
                    }

                    if (!await roleManager.RoleExistsAsync(Roles.User))
                    {
                        var userRole = new IdentityRole(Roles.User);
                        await roleManager.CreateAsync(userRole);
                    }

                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                    await DatabaseContextSeed.SeedDefaultAdmin(userManager, scope, roleManager);
                    
                }
                catch
                {
                    //TODO: log error
                }
            }
            return webHost;
        }   
    
        public static void SwaggerMiddlewareBinding(this IApplicationBuilder app, IConfiguration configuration)
        {
                        var swaggerOptions = new SwaggerOptions();
            configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => 
            { 
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); 
            
            });
        }

        public static void HealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(x => new HealthCheck
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description

                        }),
                        Duration = report.TotalDuration
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });

        }
    
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    { 
                        await context.Response.WriteAsync(new Error()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());

                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                    }
                });
            });
        }
    

    
    }

    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }        
    }



}
