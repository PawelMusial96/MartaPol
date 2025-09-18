using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MartaPol.Domain.Abstractions;
using MartaPol.Domain.Models;
using MartaPol.Infrastructure.Data;
using System.Collections.ObjectModel;

namespace MartaPol.App.ViewModels;

public partial class SendViewModel : ObservableObject
{
    private readonly ISheetService _sheets;
    private readonly IExportService _export;
    private readonly IEmailService _email;
    private readonly ISettingsService _settings;
    private readonly MartaPolDb _db;
    private readonly IDialogService _dialogs;

    [ObservableProperty] private string subject = $"Wyniki skanowania {DateTime.Now:yyyy-MM-dd}";
    [ObservableProperty] private string recipient = string.Empty;
    [ObservableProperty] private ObservableCollection<Sheet> selectedSheets = new();
    [ObservableProperty] private ObservableCollection<EmailLog> logs = new();

    public SendViewModel(ISheetService sheets, IExportService export, IEmailService email, ISettingsService settings, MartaPolDb db, IDialogService dialogs)
    {
        _sheets = sheets; _export = export; _email = email; _settings = settings; _db = db; _dialogs = dialogs;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        Recipient = (await _settings.GetAllAsync()).Recipient;
        var log = await _db.Conn.Table<EmailLog>().OrderByDescending(l => l.SentAt).Take(20).ToListAsync();
        Logs = new(log); // OK: podmiana kolekcji z powiadomieniem
    }

    [RelayCommand]
    private async Task PickSheetsAsync()
    {
        var all = await _sheets.GetSheetsAsync(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(1));
        SelectedSheets = new(new List<Sheet>(all.Take(3)));
        await _dialogs.ToastAsync($"Wybrano {SelectedSheets.Count} ark.");
    }

    [RelayCommand]
    private Task PreviewMailAsync()
    {
        var names = string.Join(", ", SelectedSheets.Select(s => s.Name));
        return _dialogs.ToastAsync($"Temat: {Subject}\nZałączniki: {names}");
    }

    [RelayCommand]
    private async Task SendAsync()
    {
        if (SelectedSheets.Count == 0)
        {
            await _dialogs.ToastAsync("Nie wybrano arkuszy.");
            return;
        }

        var fmt = (await _settings.GetAllAsync()).ExportFormat;
        var files = new List<string>();
        foreach (var s in SelectedSheets)
            files.AddRange(await _export.ExportAsync(s.Id, fmt));

        bool ok = false; string? err = null;
        try
        {
            ok = await _email.SendAsync(files, Subject, Recipient);
        }
        catch (Exception ex) { ok = false; err = ex.Message; }

        var log = new EmailLog
        {
            Id = Guid.NewGuid(),
            SentAt = DateTime.Now,
            Recipient = Recipient,
            Attachments = string.Join(", ", files.Select(Path.GetFileName)),
            Success = ok,
            Error = err
        };
        await _db.Conn.InsertAsync(log);
        Logs.Insert(0, log);
        await _dialogs.ToastAsync(ok ? "Wysłano" : $"Błąd: {err}");
    }
}

