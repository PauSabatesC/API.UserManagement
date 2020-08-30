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
using UserManagement.Domain.Enums;
using UserManagement.Services.Options;

namespace UserManagement.Infrastructure.Database
{
    public static class DatabaseContextSeed
    {
        public static async Task SeedDefaultAdmin(UserManager<User> userManager, IServiceScope scope, RoleManager<IdentityRole> roleManager)
        {
            //var config = scope.ServiceProvider.GetRequiredService<IConfiguration>(); //Does not work in runtime

            var defaultUser = new User
            {
                UserName = Environment.GetEnvironmentVariable("defusername"), //to use with docker ENV
                Email = Environment.GetEnvironmentVariable("defemail"), //to use with docker ENV
                AddedDate = DateTime.UtcNow,
            };

            if (defaultUser.UserName == null) defaultUser.UserName = "admin";
            if (defaultUser.Email == null) defaultUser.Email = "admin@admin.com";

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                var passwd = Environment.GetEnvironmentVariable("defpass") == null ? "adminadmin" : Environment.GetEnvironmentVariable("defpass");
                await userManager.CreateAsync(defaultUser, passwd);
                //await roleManager.
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin);
            }

            //just seeding for pagination testing purposes
            for (int i = 0; i < 12; i++) 
                await userManager.CreateAsync(new User { UserName = string.Concat("user", i), Email = string.Concat("user", i, "@user.com") }, "useruser");

        }
    }
}
