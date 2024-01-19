using System;
using Microsoft.Extensions.DependencyInjection;
using PzomPatch.Services.Dialog;
using PzomPatch.Services.Process;
using PzomPatch.Services.Settings;
using PzomPatch.Services.Zomboid;
using PzomPatch.ViewModels;
using PzomPatch.Views;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace PzomPatch.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLogger(this IServiceCollection service)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                   @"\PzomPatch\Logs";
        var formatter = new CompactJsonFormatter();
        var log = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Console(formatter)
            .WriteTo.File(formatter, path + @"\log_.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        service.AddSingleton<ILogger>(log);

        return service;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection service)
    {
        service.AddSingleton<ISettingsService, SettingsService>();
        service.AddSingleton<IDialogService, DialogService>();
        service.AddSingleton<IProcessService, ProcessService>();
        service.AddSingleton<IZomboidService, ZomboidService>();

        return service;
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection service)
    {
        service.AddSingleton<MainViewModel>();

        return service;
    }

    public static IServiceCollection RegisterViews(this IServiceCollection service)
    {
        service.AddTransient<MainView>();

        return service;
    }
}