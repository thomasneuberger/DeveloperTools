using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusTool.ServiceBus.Contract.Interfaces;

namespace ServiceBusTool.ServiceBus.Services;

[ExcludeFromCodeCoverage]
public static class Bootstrap
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionManager, ConnectionManager>();
    }
}