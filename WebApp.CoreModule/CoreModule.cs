using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using WebApp.Extensibility.Initialization;

namespace WebApp.CoreModule
{
    public static class CoreModule
    {
        public static void Configure(IApplicationBuilder app)
        {
        }

        public static void Initialize(IServiceProvider services)
        {
            IInitializationManager manager = services.GetRequiredService<IInitializationManager>();
            manager.RegisterInitializer<LongRunningStore>();
            manager.RegisterInitializer<InitializationTask>();
            manager.RegisterInitializer<LazyStoreProvider>();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<InitializationTask>();
            services.AddSingleton<LongRunningStore>();
            services.AddSingleton<LazyStoreProvider>();

            // This second level indirection is only required when we have to support initialization and want to hide
            // the details from the interface class
            services.AddSingleton<ILazyStoreProvider>(services => services.GetRequiredService<LazyStoreProvider>());
        }
    }
}
