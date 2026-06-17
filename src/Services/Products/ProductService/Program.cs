using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductService.Data;
using ProductService.Infrastructure.Contexts;
using System.Threading.Tasks;

namespace ProductService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ProductDatabaseContext>();
            await dbContext.Database.MigrateAsync();

            await DataSeeder.Seed(host.Services);

            host.Run();

        }

        //public static void Main(string[] args)
        //{
        //    var host = CreateHostBuilder(args).Build();
        //    host.Run();

        //    var sp = host.Services;
        //}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
