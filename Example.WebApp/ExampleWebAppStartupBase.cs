using Example.WebApp.Core;
using Example.WebApp.Middleware;
using Example.WebApp.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using WebApp.CoreModule;
using WebApp.Extensibility.Initialization;

namespace Example.WebApp
{
    public abstract class ExampleWebAppStartupBase
    {
        public virtual void Configure(IApplicationBuilder app)
        {
            // Configure your request pipeline here ....
            app.UseRouting();
            app.UseMiddleware<CircuitBreakerMiddleware>();
            app.UseEndpoints(builder =>
            {
                builder.MapCustomHealthEndpoints();
            });

            // example ...
            CoreModule.Configure(app);

            // The rest of your pipeline goes after this ...
            // ...
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ApplicationLifetimeService>();
            services.AddSingleton<CircuitBreakerMiddleware>();
            services.AddSingleton<ApplicationStateProvider>();
            services.AddSingleton<InitializationManager>();

            // Other Services
            services.AddSingleton<IApplicationStateProvider>(services => services.GetRequiredService<ApplicationStateProvider>());
            services.AddSingleton<IInitializationManager>(services => services.GetRequiredService<InitializationManager>());

            // Since we call this here ... it allows the core module to override any services defined above ...
            CoreModule.ConfigureServices(services);

            // And more 'critical' serivces now can be registered below.
        }
    }
}
