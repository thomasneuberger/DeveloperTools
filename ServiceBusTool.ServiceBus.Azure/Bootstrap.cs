using Microsoft.Extensions.DependencyInjection;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace ServiceBusTool.ServiceBus.Azure;

[ExcludeFromCodeCoverage]
public static class Bootstrap
{
    public static void AddAzure(this IServiceCollection services)
    {
        services.AddScoped<IServiceBus, AzureServiceBus>();
    }
}
