using WebApp.Extensibility.Initialization;

namespace WebApp.CoreModule
{
    internal class InitializationTask : IAsyncInitializable, IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("InitializationTask is disposing...");
        }

        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("InitializationTask is initializing...");

            return Task.CompletedTask;
        }
    }
}
