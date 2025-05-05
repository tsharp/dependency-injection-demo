using WebApp.Extensibility.Initialization;

namespace WebApp.CoreModule
{
    [InitializationSequence(7)]
    internal class LongRunningStore : ILazyStoreProvider, IAsyncInitializable
    {
        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("LongRunningStore initialized.");
            return Task.CompletedTask;
        }
    }
}
