using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ServiceBusTool.ServiceBus.ViewModels;

[ExcludeFromCodeCoverage]
public static class Bootstrap
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<IndexViewModel>();
        services.AddScoped<ConnectionsViewModel>();
        services.AddScoped<MessagesViewModel>();
    }
}
