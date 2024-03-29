﻿using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using ServiceBusTool.ServiceBus.Azure;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Managers;
using ServiceBusTool.ServiceBus.ViewModels;
using ServiceBusTool.Services;

namespace ServiceBusTool;

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

        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
        });

        builder.Services.AddManagers();
        builder.Services.AddViewModels();
        builder.Services.AddAzure();

        return builder.Build();
    }
}