using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Platform;
using Avalonia.Styling;
using Avalonia.Themes.Default;
using Avalonia.Themes.Fluent;
using Toradex.Views;

namespace Toradex;

public class App : Application
{
    public static CancellationTokenSource LottieCancelationSource { get; } = new();
    //public static LottieSplashToDrm StaticLottieSplashToDrm { get; set; }

    public override void Initialize()
    {
        PerformanceCounter.Step("App-Initialize");
        AvaloniaXamlLoader.Load(this);
        PerformanceCounter.Step("App AvaloniaXamlLoader loaded");
    }

    public override void OnFrameworkInitializationCompleted()
    {
        AvaloniaLocator.CurrentMutable
            .Bind<IFontManagerImpl>().ToConstant(new CustomFontManagerImpl());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            singleView.MainView = new Viewbox { Child = new MainView() };
        base.OnFrameworkInitializationCompleted();
        StartServices();
    }

    private void StartServices()
    {
        // TODO
    }
}
