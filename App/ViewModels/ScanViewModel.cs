using MartaPol.Domain.Abstractions;
using MartaPol.Domain.Models;
using MartaPol.Domain.Parsing;
using MartaPol.Domain.Enums;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace MartaPol.App.ViewModels;

public partial class ScanViewModel : ObservableObject
{
    private readonly IBarcodeWeightParser _gs1;
    private readonly Ean13VariableWeightParser _eanVar;
    private readonly ISettingsService _settings;
    private readonly ISheetService _sheets;
    private readonly IDeviceFeedback _feedback;
    private readonly IDialogService _dialogs;
    private readonly IDateTimeProvider _clock;

    private DateTime _lastScan = DateTime.MinValue;

    [ObservableProperty]
    private ObservableCollection<ScanRecord> lastFive = new();

    [ObservableProperty]
    private int sessionCount;

    private readonly List<ScanRecord> _session = new();

    public ScanViewModel(IBarcodeWeightParser gs1, Ean13VariableWeightParser eanVar, ISettingsService settings,
        ISheetService sheets, IDeviceFeedback feedback, IDialogService dialogs, IDateTimeProvider clock)
    {
        _gs1 = gs1; _eanVar = eanVar; _settings = settings; _sheets = sheets; _feedback = feedback; _dialogs = dialogs; _clock = clock;
    }

    public Task OnAppearingAsync() => _settings.EnsureLoadedAsync();

    public async void HandleDetectedCode(string code)
    {
        if ((DateTime.UtcNow - _lastScan).TotalMilliseconds < 500) return;
        _lastScan = DateTime.UtcNow;

        decimal? kg = null;
        var res = _gs1.TryParseWeight(code);
        if (res.Success) kg = res.WeightKg;
        else
        {
            var ean = _eanVar.TryParse(code, await _settings.GetEanRulesAsync());
            if (ean.Success) kg = ean.WeightKg;
        }

        if (kg is null)
        {
            var input = await _dialogs.PromptAsync("Nie rozpoznano wagi", $"Kod: {code}\nPodaj wagę w kg:");
            if (decimal.TryParse(input, out var manualKg)) kg = Math.Round(manualKg, 3);
            else return;
        }

        var rec = new ScanRecord
        {
            Id = Guid.NewGuid(),
            Code = code,
            WeightKg = Math.Round(kg!.Value, 3),
            ScannedAt = _clock.NowLocal,
        };

        var policy = await _settings.GetDuplicatePolicyAsync();
        bool isDup = _session.Any(x => x.Code == code);
        if (isDup)
        {
            switch (policy)
            {
                case DuplicatePolicy.Ignore:
                    return;
                case DuplicatePolicy.Ask:
                    var ok = await _dialogs.ConfirmAsync("Duplikat", "Kod już istnieje w sesji. Dodać drugi wpis?");
                    if (!ok) return;
                    break;
                case DuplicatePolicy.Append:
                default:
                    break;
            }
        }

        _session.Add(rec);
        SessionCount = _session.Count;
        LastFive.Insert(0, rec);
        while (LastFive.Count > 5) LastFive.RemoveAt(LastFive.Count - 1);

        var (sound, vib) = await _settings.GetFeedbackAsync();
        if (vib) _feedback.Vibrate(100);
        if (sound) _feedback.Beep();

        var limit = await _settings.GetSheetRecordLimitAsync();
        if (limit > 0 && _session.Count >= limit)
        {
            await FinishSessionAsync();
        }
    }

    [RelayCommand]
    private async Task FinishSessionAsync()
    {
        if (_session.Count == 0)
        {
            await _dialogs.ToastAsync("Brak danych w sesji");
            return;
        }

        await _sheets.SaveCurrentSessionAsSheetAsync(_session.ToList());
        _session.Clear();
        LastFive.Clear();
        SessionCount = 0;
        await _dialogs.ToastAsync("Arkusz utworzony");
    }

    [RelayCommand]
    private Task ToggleDetectingAsync() => _dialogs.ToastAsync("(Podgląd) wstrzymanie/wznowienie – kontrolowane przez widok kamery.");
}
