using Microsoft.Extensions.Logging;
using ServiceBusTool.Blazor.Services;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Managers;
using ServiceBusTool.ServiceBus.ViewModels;

namespace ServiceBusTool.Blazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Logging
                .AddConsole()
                .AddDebug();

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            
            builder.Services.AddSingleton<IKeyValueStore, KeyValueStore>();

            builder.Services.AddManagers();
            builder.Services.AddViewModels();

            return builder.Build();
        }
    }
}