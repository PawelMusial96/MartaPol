using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MartaPol.Domain.Abstractions;
using MartaPol.Domain.Enums;
using MartaPol.Domain.Models;
using System.Collections.ObjectModel;

namespace MartaPol.App.ViewModels;

public partial class SheetsViewModel : ObservableObject
{
    private readonly ISheetService _sheets;
    private readonly IExportService _export;
    private readonly ISettingsService _settings;
    private readonly IDialogService _dialogs;

    [ObservableProperty] ObservableCollection<Sheet> sheets = new();
    [ObservableProperty] string? query;
    [ObservableProperty] DateTime fromDate = DateTime.Now.AddDays(-7);
    [ObservableProperty] DateTime toDate = DateTime.Now.AddDays(1);

    public SheetsViewModel(ISheetService s, IExportService e, ISettingsService settings, IDialogService dialogs)
    { _sheets = s; _export = e; _settings = settings; _dialogs = dialogs; _ = RefreshAsync(); }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        var list = await _sheets.GetSheetsAsync(fromDate, toDate, query);
        Sheets = new(list);
    }

    [RelayCommand]
    private async Task ExportAsync(Sheet sheet)
    {
        var fmt = await _settings.GetExportFormatAsync();
        var files = await _export.ExportAsync(sheet.Id, fmt);
        await _dialogs.ToastAsync($"Wyeksportowano: {string.Join(", ", files.Select(Path.GetFileName))}");
    }

    [RelayCommand]
    private async Task PreviewAsync(Sheet sheet)
    {
        var recs = await _sheets.GetSheetRecordsAsync(sheet.Id);
        var msg = string.Join("\n", recs.Take(10).Select(r => $"{r.ScannedAt:HH:mm:ss} {r.Code} {r.WeightKg:F3} kg"));
        await _dialogs.ToastAsync(msg.Length==0?"Pusty arkusz":msg);
    }
}
