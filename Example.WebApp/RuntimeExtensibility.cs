using System.Reflection;

using Example.WebApp.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WebApp.CoreModule;
using WebApp.Extensibility.Initialization;

namespace Example.WebApp
{
    public static class RuntimeExtensibility
    {
        public static async Task<IHost> InitializeHostAsync(this IHost host)
        {
            // 10 minutes is a long time to wait for a resource server to initialize, but this is just an example.
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            var cancellationToken = cancellationTokenSource.Token;

            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            // Find a better way to automate this ...
            CoreModule.Initialize(host.Services);

            InitializationManager initializationManager = host.Services.GetRequiredService<InitializationManager>();
            await initializationManager.InitializeAsync(cancellationToken);

            return host;
        }
    }
}
