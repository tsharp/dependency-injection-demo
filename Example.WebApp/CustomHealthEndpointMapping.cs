using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using WebApp.Extensibility.Initialization;

namespace Example.WebApp
{
    internal static class CustomHealthEndpointMapping
    {
        internal static IEndpointRouteBuilder MapCustomHealthEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var pipeline = endpoints
                .CreateApplicationBuilder()
                .Use(async (HttpContext context, RequestDelegate next) =>
                {
                    if (context.Request.Path.StartsWithSegments("/healthz", StringComparison.OrdinalIgnoreCase, out PathString remaining))
                    {
                        var stateProvider = context.RequestServices.GetRequiredService<IApplicationStateProvider>();
                        var pathInfo = remaining.ToString().TrimStart('/');

                        if (pathInfo.Equals("ready", StringComparison.OrdinalIgnoreCase))
                        {
                            if (stateProvider.State == ApplicationState.Running)
                            {
                                context.Response.StatusCode = 200;
                            }
                            else
                            {
                                context.Response.StatusCode = 503;
                            }

                            context.Response.ContentType = "text/plain";
                            await context.Response.WriteAsync(stateProvider.State.ToString());
                            await context.Response.CompleteAsync();
                            return;
                        }

                        if (pathInfo.Equals("live", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Response.StatusCode = 200;
                            context.Response.ContentType = "text/plain";
                            await context.Response.WriteAsync("OK");
                            await context.Response.CompleteAsync();
                            return;
                        }

                        if (pathInfo.Equals("status", StringComparison.OrdinalIgnoreCase))
                        {
                            await context.Response.WriteAsJsonAsync(new
                            {
                                state = stateProvider.State.ToString(),
                                healthStatus = stateProvider.HealthStatus.ToString(),
                                ready = stateProvider.State == ApplicationState.Running,
                                live = stateProvider.State != ApplicationState.Crashed && stateProvider.State != ApplicationState.Terminated,
                                timestamp = DateTime.UtcNow,
                            });

                            return;
                        }
                    }

                    await next(context);
                })
                .Build();

            endpoints.Map($"/healthz/{{*pathInfo}}", pipeline)
                .WithDescription("WebHost Health Request Handler");

            return endpoints;
        }
    }
}
