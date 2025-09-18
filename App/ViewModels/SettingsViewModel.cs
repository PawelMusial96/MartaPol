using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MartaPol.Domain.Abstractions;
using MartaPol.Domain.Enums;
using MartaPol.Domain.Models;
using System.Collections.ObjectModel;

namespace MartaPol.App.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settings;
    private readonly IDialogService _dialogs;

    [ObservableProperty] public SettingsModel model = new();
    [ObservableProperty] public string smtpPassword = string.Empty;

    public ObservableCollection<string> Themes { get; } = new(new[] { "System", "Light", "Dark" });
    public ObservableCollection<string> Encryptions { get; } = new(new[] { "STARTTLS", "TLS", "None" });
    public ObservableCollection<DuplicatePolicy> DuplicatePolicies { get; } = new(Enum.GetValues<DuplicatePolicy>());
    public ObservableCollection<ExportFormat> ExportFormats { get; } = new(Enum.GetValues<ExportFormat>());

    [ObservableProperty] public DuplicatePolicy selectedDuplicatePolicy;
    [ObservableProperty] public ExportFormat selectedExportFormat;
    [ObservableProperty] public string theme = "System";

    public SettingsViewModel(ISettingsService settings, IDialogService dialogs)
    { _settings = settings; _dialogs = dialogs; _ = LoadAsync(); }

    private async Task LoadAsync()
    {
        await _settings.EnsureLoadedAsync();
        Model = await _settings.GetAllAsync();
        SelectedDuplicatePolicy = Model.DuplicatePolicy;
        SelectedExportFormat = Model.ExportFormat;
        Theme = Model.Theme;
        SmtpPassword = await _settings.GetSmtpPasswordAsync() ?? string.Empty;
    }

    partial void OnSelectedDuplicatePolicyChanged(DuplicatePolicy value) => Model.DuplicatePolicy = value;
    partial void OnSelectedExportFormatChanged(ExportFormat value) => Model.ExportFormat = value;
    partial void OnThemeChanged(string value) => Model.Theme = value;

    [RelayCommand]
    private async Task SaveAsync()
    {
        await _settings.SaveAsync(Model, string.IsNullOrWhiteSpace(SmtpPassword) ? null : SmtpPassword);
        await _dialogs.ToastAsync("Zapisano ustawienia.");
    }
}
