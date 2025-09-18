using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui;
using MartaPol.Domain.Abstractions;
using MartaPol.Domain.Parsing;
using MartaPol.Infrastructure.Data;
using MartaPol.Infrastructure.Services;
using MartaPol.Infrastructure.Services.Export;
using MartaPol.Infrastructure.Services.Email;
using MartaPol.Infrastructure.Services.Sheets;
using MartaPol.App.ViewModels;
using MartaPol.App.Views;
using ZXing.Net.Maui.Controls;

namespace MartaPol.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseBarcodeReader() // ZXing.Net.Maui
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Db
        builder.Services.AddSingleton<MartaPolDb>();

        // Parsers
        builder.Services.AddSingleton<IBarcodeWeightParser, Gs1Parser>();
        builder.Services.AddSingleton<Ean13VariableWeightParser>();

        // Repos/services
        builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<ISheetService, SheetService>();
        builder.Services.AddSingleton<IExportService, ExportMuxService>();
        builder.Services.AddSingleton<IEmailService, MailKitEmailService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IDeviceFeedback, DeviceFeedback>();

#if ANDROID
        builder.Services.AddSingleton<MartaPol.App.Platforms.Android.Services.SoundServiceAndroid>();
#endif

        // VMs
        builder.Services.AddTransient<ScanViewModel>();
        builder.Services.AddTransient<SheetsViewModel>();
        builder.Services.AddTransient<SheetDetailViewModel>();
        builder.Services.AddTransient<SendViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        // Pages
        builder.Services.AddTransient<ScanPage>();
        builder.Services.AddTransient<SheetsPage>();
        builder.Services.AddTransient<SheetDetailPage>();
        builder.Services.AddTransient<SendPage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder.Build();
    }
}
