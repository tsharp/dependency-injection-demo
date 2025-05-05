using Example.WebApp;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WebApp.Extensibility.Initialization;

namespace ConsoleHostedWebApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var host = BuildHost(args))
            {
                await host.InitializeHostAsync();

                // Run and wait until the host is stopped
                await host.RunAsync();
            }
        }

        private static IHost BuildHost(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            if (OperatingSystem.IsWindows() && !Environment.UserInteractive)
            {
                Console.WriteLine("Running As Windows Service");
                builder.UseWindowsService();
            }

            if (OperatingSystem.IsLinux() && !Environment.UserInteractive)
            {
                Console.WriteLine("Running As Linux Service");
                builder.UseSystemd();
            }

            IHost host = builder
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel();

                webBuilder.ConfigureServices(services =>
                {
                });

                webBuilder.Configure(appBuilder =>
                {
                    appBuilder.UseRouting();
                });

                webBuilder.UseStartup<ExampleWebAppStartup>();
            })
            .Build();

            return host;
        }
    }
}
