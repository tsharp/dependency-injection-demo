
using System.Reflection;

using Example.WebApp.Core;

using Microsoft.Extensions.Hosting;

using WebApp.Extensibility.Initialization;

namespace Example.WebApp.Services
{
    internal class ApplicationLifetimeService : IHostedService, IDisposable
    {
        private ApplicationStateProvider _applicationStateProvider;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _applicationStateTask = Task.CompletedTask;

        public ApplicationLifetimeService(
            ApplicationStateProvider applicationStateProvider,
            IEnumerable<IAsyncInitializable> initializationTasks)
        {
            _applicationStateProvider = applicationStateProvider;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _applicationStateProvider.State = ApplicationState.Initializing;

                _applicationStateTask = this
                    ._applicationStateProvider
                    .ExecuteAsync(_cancellationTokenSource.Token);

                _applicationStateProvider.State = ApplicationState.Running;
            }
            catch
            {
                // Logging, etc.
                _applicationStateProvider.State = ApplicationState.Crashed;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            _applicationStateProvider.State = ApplicationState.ShuttingDown;

            // Wait for things to gracefully shut down ...
            // you could probably wait for connections to close, etc.
            while (!cancellationToken.IsCancellationRequested && !_applicationStateTask.IsCompleted)
            {
                await Task.Delay(100);
            }

            _applicationStateProvider.State = ApplicationState.Terminated;
        }
    }
}
