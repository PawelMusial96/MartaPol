using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using MartaPol.App.Views;

namespace MartaPol.App;

public partial class App : Application
{
    public App(ScanPage scanPage)
    {
        InitializeComponent();
        MainPage = new AppShell(scanPage);
    }
}

public class AppShell : Shell
{
    public AppShell(ScanPage scanPage)
    {
        Items.Add(new TabBar
        {
            Items =
            {
                new ShellContent{ Title="Skanuj", ContentTemplate = new DataTemplate(()=>scanPage)},
                new ShellContent{ Title="Arkusze", ContentTemplate = new DataTemplate(()=>ServiceHelper.GetService<SheetsPage>()) },
                new ShellContent{ Title="Wysyłka", ContentTemplate = new DataTemplate(()=>ServiceHelper.GetService<SendPage>()) },
                new ShellContent{ Title="Ustawienia", ContentTemplate = new DataTemplate(()=>ServiceHelper.GetService<SettingsPage>()) }
            }
        });
    }
}

public static class ServiceHelper
{
    public static T GetService<T>() where T : notnull
        => Current.GetRequiredService<T>();

    public static IServiceProvider Current =>
#if ANDROID
        MauiApplication.Current.Services;
#elif IOS || MACCATALYST
        MauiUIApplicationDelegate.Current.Services;
#elif WINDOWS10_0_17763_0_OR_GREATER
        MauiWinUIApplication.Current.Services;
#else
        throw new PlatformNotSupportedException("Unknown platform");
#endif
}