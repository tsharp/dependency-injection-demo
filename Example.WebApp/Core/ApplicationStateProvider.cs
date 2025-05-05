using Microsoft.Extensions.Diagnostics.HealthChecks;

using WebApp.Extensibility.Initialization;

namespace Example.WebApp.Core
{
    internal class ApplicationStateProvider : IApplicationStateProvider
    {
        public HealthStatus HealthStatus => HealthStatus.Healthy;

        public ApplicationState State { get; set; } = ApplicationState.Uninitialized;

        internal async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Provides a way for health checking to occur ... if this crashes, then the application is in a bad state.

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
