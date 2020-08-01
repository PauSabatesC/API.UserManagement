using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Enums;
using UserManagement.Infrastructure.Database;

namespace API.UserManagement.Extensions
{
    public static class AspnetCoreExtensions
    {
        public static async Task<IWebHost> CreateDatabase<T>(this IWebHost webHost) where T : DbContext
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
    }
}
