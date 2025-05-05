using Example.WebApp.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using WebApp.Extensibility.Initialization;

namespace Example.WebApp.Middleware
{
    internal class CircuitBreakerMiddleware : IMiddleware
    {
        private readonly IApplicationStateProvider applicationStateProvider;

        public CircuitBreakerMiddleware(IApplicationStateProvider applicationStateProvider)
        {
            this.applicationStateProvider = applicationStateProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            // Allow administrative endpoints and health endpoints to bypass circuit breakers
            // since they are used to check the health of the application and provide administrative
            // functionality therefore must not be blocked by the circuit breaker to ensure
            // the application can be brought back to a healthy state and to indicate health status
            // to external monitoring.
            if (context.Request.Path.StartsWithSegments("/healthz") ||
                context.Request.Path.StartsWithSegments("/admin"))
            {
                await next(context);
                return;
            }

            if (this.applicationStateProvider.HealthStatus == HealthStatus.Unhealthy ||
                this.applicationStateProvider.State != ApplicationState.Running)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Service Unavailable");
                await context.Response.CompleteAsync();
                return;
            }

            await next(context);
        }
    }
}
