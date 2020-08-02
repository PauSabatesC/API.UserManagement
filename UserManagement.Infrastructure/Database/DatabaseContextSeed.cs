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
            //var config = scope.ServiceProvider.GetRequiredService<IConfiguration>(); //Does not work in runtime

            var defaultUser = new User
            {
                UserName = Environment.GetEnvironmentVariable("defusername"),
                AddedDate = DateTime.UtcNow,
                Email = Environment.GetEnvironmentVariable("defemail")
            };


            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, Environment.GetEnvironmentVariable("defpass"));
                //await roleManager.
            }

        }
    }
}
