using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QNTM.API.Data;
using QNTM.API.Models;

namespace QNTM.API
{
    #pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    // var userManager = services.GetRequiredService<UserManager<User>>();
                    context.Database.Migrate();
                    Seed.SeedUsers(context);
                }
                catch(Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An Error Occured During Migration");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
    #pragma warning restore CS1591
}
