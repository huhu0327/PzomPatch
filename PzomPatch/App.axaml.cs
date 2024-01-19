using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PzomPatch.Extensions;
using PzomPatch.Views;

namespace PzomPatch;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var serviceProvider = new ServiceCollection()
            .RegisterLogger()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .BuildServiceProvider();

        Ioc.Default.ConfigureServices(serviceProvider);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        desktop.MainWindow = Ioc.Default.GetRequiredService<MainView>();

        base.OnFrameworkInitializationCompleted();
    }
}