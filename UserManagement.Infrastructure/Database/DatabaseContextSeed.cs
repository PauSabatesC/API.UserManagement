using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;
using UserManagement.Services.Options;

namespace UserManagement.Infrastructure.Database
{
    public static class DatabaseContextSeed
    {
        public static async Task SeedDefaultAdmin(UserManager<User> userManager, IServiceScope scope, RoleManager<IdentityRole> roleManager)
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var defaultUser = new User
            {
                UserName = config.GetConnectionString("AdminUsername"),
                AddedDate = DateTime.UtcNow,
                Email = config.GetConnectionString("AdminEmail")
            };


            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, config.GetConnectionString("AdminPassword"));
                //await roleManager.
            }

        }
    }
}
