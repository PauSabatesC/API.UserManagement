using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.UserManagement.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserManagement.Infrastructure.Database;

namespace API.LoginAndRegister
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            await host.CreateDatabase<DataContext>();

            await host.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
         WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>();


    }
}
