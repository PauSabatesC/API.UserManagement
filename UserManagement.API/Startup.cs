using System.Collections.Generic;
using System.Text;
using UserManagement.Domain.Entities;
using UserManagement.Domain.RepositoryInterfaces;
using UserManagement.Services.Options;
using UserManagement.Services.Boundaries;
using UserManagement.Services;
using UserManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserManagement.Domain.Enums;
using System;
using UserManagement.API.FiltersMiddleware.AuthorizationMiddlewares.Policies;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FluentValidation.AspNetCore;
using UserManagement.API.FiltersMiddleware.AuthorizationMiddlewares.RequestsValidators;
using System.Reflection;
using UserManagement.API.Services;
using UserManagement.Services.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using UserManagement.API.Controllers.HealthChecks;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace UserManagement.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ////CONTROLLERS
            services.AddControllers(options =>
            {
                options.Filters.Add<RequestValidationFilter>();
            })
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<Startup>());

            ////AUTOMAPPER
            Type[] profileAssemblyTypes = new[] {typeof(Startup), typeof(UserManagement.Services.IdentityService)};
            services.AddAutoMapper(profileAssemblyTypes);

            ////SWAGGER
            services.AddSwaggerGen( x => 
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "OpenUserManager API", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0] }
                };

                x.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {new OpenApiSecurityScheme{Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }}, new List<string>()}
                });


            });

            ////JWT + Authorization
            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false

            };
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.MustBeEnterpriseEmail, policy =>
                {
                    policy.AddRequirements(new EmailDomainRequirement("email.com"));
                });
            });

            services.AddSingleton<IAuthorizationHandler, EmailDomainHandler>();

            ////DB
            services.AddDbContext<DataContext>(dbContextOption => dbContextOption.UseSqlServer(Configuration.GetConnectionString("SQLServerDb")));
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>();

            ////HEALTH CHECKS
            services.AddHealthChecks()
                .AddDbContextCheck<DataContext>();

            ////VERSIONING
            services.AddApiVersioning(cnfg =>
            {
                cnfg.DefaultApiVersion = new ApiVersion(1, 0);
                cnfg.AssumeDefaultVersionWhenUnspecified = true;
                cnfg.ReportApiVersions = true;
                cnfg.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            ////OTHER SERVICES DI
            services.AddScoped<IUsersService,UsersService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddTransient<IUsersAdminManagementRepository, UsersAdminManagementRepository>();        
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUriService<IPagination>>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absolutUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                return new UriService<IPagination>(absolutUri);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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

            //Swagger middleware binding
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => 
            { 
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); 
            
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
