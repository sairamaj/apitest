using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Console.WriteLine($"Current Environment : {(string.IsNullOrEmpty(environment) ? "Development" : environment)}");
            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();

            Console.WriteLine(config["message"]);
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IWebJobConfiguration>(config.GetSection("WebJobConfiguration").Get<WebJobConfiguration>());

            services.AddTransient<WebJobEntryPoint>();

            var builder = new HostBuilder()
                    .ConfigureWebJobs(webJobConfiguration =>
                    {
                        webJobConfiguration.AddTimers();
                        webJobConfiguration.AddAzureStorageCoreServices();
                    })
                    .ConfigureServices(serviceCollection => serviceCollection.AddTransient<AppRunner>())
                    .Build();
            builder.Run();
        }
    }
}
