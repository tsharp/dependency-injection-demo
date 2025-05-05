using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApp.Extensibility.Initialization
{
    public interface IApplicationStateProvider
    {
        HealthStatus HealthStatus { get; }

        ApplicationState State { get; }
    }
}
