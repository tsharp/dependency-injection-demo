using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WebApp.Extensibility.Initialization;

namespace Example.WebApp.Core
{
    internal class InitializationManager : IInitializationManager
    {
        private readonly IServiceProvider services;
        private readonly HashSet<IAsyncInitializable> initializers = new HashSet<IAsyncInitializable>();

        public InitializationManager(IServiceProvider services)
        {
            this.services = services;
        }

        public void RegisterInitializer<T>() where T : IAsyncInitializable
        {
            var initializer = services.GetRequiredService<T>();
            initializers.Add(initializer);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var orderedInitialization = initializers
             .OrderBy(x => x.GetType().GetCustomAttribute<InitializationSequenceAttribute>()?.Sequence ?? int.MaxValue)
             .ToArray();

            foreach (var initializer in initializers)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException("Initialization was canceled.");
                }

                await initializer.InitializeAsync(cancellationToken);
            }
        }
    }
}
