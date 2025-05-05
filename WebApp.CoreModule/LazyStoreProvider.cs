
using WebApp.Extensibility.Initialization;

namespace WebApp.CoreModule
{
    [InitializationSequence(0)]
    internal class LazyStoreProvider : ILazyStoreProvider
    {
        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("LazyStoreProvider initialized.");

            return Task.CompletedTask;
        }
    }
}
