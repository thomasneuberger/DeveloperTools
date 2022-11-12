using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;

namespace ServiceBusTool.ServiceBus.Managers;

[ExcludeFromCodeCoverage]
public static class Bootstrap
{
    public static void AddManagers(this IServiceCollection services)
    {
        services.AddSingleton<IKeyValueListManager<Connection>, ConnectionManager>();
        services.AddSingleton<IKeyValueListManager<MessageDefinition>, MessageDefinitionManager>();
        services.AddSingleton<IKeyValueListManager<ParameterDefinition>, ParameterDefinitionManager>();
    }
}