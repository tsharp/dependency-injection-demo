using Example.WebApp.Core;

using Microsoft.Extensions.Hosting;

using WebApp.CoreModule;
using WebApp.Extensibility.Initialization;

namespace Example.WebApp.Services
{
    internal class ApplicationLifetimeService : IHostedLifecycleService, IDisposable
    {
        private readonly InitializationManager _initializationManager;
        private readonly ApplicationStateProvider _applicationStateProvider;
        private readonly IServiceProvider _services;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _applicationStateTask = Task.CompletedTask;

        public ApplicationLifetimeService(
            IServiceProvider services,
            InitializationManager initializationManager,
            ApplicationStateProvider applicationStateProvider)
        {
            _services = services;
            _initializationManager = initializationManager;
            _applicationStateProvider = applicationStateProvider;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task StartedAsync(CancellationToken cancellationToken)
        {
            _applicationStateProvider.State = ApplicationState.Running;
            return Task.CompletedTask;
        }

        public async Task StartingAsync(CancellationToken cancellationToken)
        {
            try
            {
                // This module thingy is a bit odd, but it currently serves the purpose of registering
                // tasks for initialization with the initialization manager.
                CoreModule.Initialize(_services);

                _applicationStateProvider.State = ApplicationState.Initializing;

                await _initializationManager.InitializeAsync(cancellationToken);

                _applicationStateTask = this
                    ._applicationStateProvider
                    .ExecuteAsync(_cancellationTokenSource.Token);
            }
            catch
            {
                // Logging, etc.
                _applicationStateProvider.State = ApplicationState.Crashed;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            _applicationStateProvider.State = ApplicationState.Terminated;
            return Task.CompletedTask;
        }

        public async Task StoppingAsync(CancellationToken cancellationToken)
        {
            _applicationStateProvider.State = ApplicationState.ShuttingDown;

            _cancellationTokenSource.Cancel();

            // Wait for things to gracefully shut down ...
            // you could probably wait for connections to close, etc.
            while (!cancellationToken.IsCancellationRequested &&
                    !_applicationStateTask.IsCompleted)
            {
                await Task.Delay(100);
            }
        }
    }
}
