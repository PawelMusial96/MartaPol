#if ANDROID
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using MartaPol.Domain.Abstractions;
using MartaPol.Domain.Enums;
using MartaPol.Domain.Models;
using Microsoft.Maui.ApplicationModel;  // SecureStorage

namespace MartaPol.Infrastructure.Services;

public class SettingsService : ISettingsService
{
    private const string SettingsKey = "settings_json";

    public Task EnsureLoadedAsync() => Task.CompletedTask;

    public async Task<IEnumerable<EanVariableWeightRule>> GetEanRulesAsync()
    {
        // Jeœli masz repozytoria – wstaw swój odczyt.
        return await Task.FromResult<IEnumerable<EanVariableWeightRule>>(new[]
        {
            new EanVariableWeightRule()
        });
    }

    public async Task<DuplicatePolicy> GetDuplicatePolicyAsync()
        => (await GetAllAsync()).DuplicatePolicy;

    public async Task<(bool sound, bool vib)> GetFeedbackAsync()
    {
        var s = await GetAllAsync();
        return (s.Beep, s.Vibrate);
    }

    public async Task<int> GetSheetRecordLimitAsync()
        => (await GetAllAsync()).SheetRecordLimit;

    public async Task<ExportFormat> GetExportFormatAsync()
        => (await GetAllAsync()).ExportFormat;

    public async Task<SettingsModel> GetAllAsync()
    {
        try
        {
            var json = await SecureStorage.GetAsync(SettingsKey);
            if (!string.IsNullOrWhiteSpace(json))
            {
                var model = JsonSerializer.Deserialize<SettingsModel>(json);
                if (model != null) return model;
            }
        }
        catch { /* ignore */ }
        return new SettingsModel();
    }

    public async Task SaveAsync(SettingsModel settings, string? smtpPasswordPlain = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(settings);
            await SecureStorage.SetAsync(SettingsKey, json);
        }
        catch { /* ignore */ }

        if (!string.IsNullOrEmpty(smtpPasswordPlain))
        {
            try { await SecureStorage.SetAsync(settings.SmtpPasswordKey, smtpPasswordPlain); } catch { }
        }
    }

    public async Task<string?> GetSmtpPasswordAsync()
    {
        try
        {
            var s = await GetAllAsync();
            return await SecureStorage.GetAsync(s.SmtpPasswordKey);
        }
        catch { return null; }
    }
}
#else
using System.Collections.Generic;
using System.Threading.Tasks;
using MartaPol.Domain.Abstractions;
using MartaPol.Domain.Enums;
using MartaPol.Domain.Models;

namespace MartaPol.Infrastructure.Services;

public class SettingsService : ISettingsService
{
    public Task EnsureLoadedAsync() => Task.CompletedTask;
    public Task<IEnumerable<EanVariableWeightRule>> GetEanRulesAsync() => Task.FromResult<IEnumerable<EanVariableWeightRule>>(new[] { new EanVariableWeightRule() });
    public Task<DuplicatePolicy> GetDuplicatePolicyAsync() => Task.FromResult(DuplicatePolicy.Ask);
    public Task<(bool sound, bool vib)> GetFeedbackAsync() => Task.FromResult((true, true));
    public Task<int> GetSheetRecordLimitAsync() => Task.FromResult(1000);
    public Task<ExportFormat> GetExportFormatAsync() => Task.FromResult(ExportFormat.Csv);
    public Task<SettingsModel> GetAllAsync() => Task.FromResult(new SettingsModel());
    public Task SaveAsync(SettingsModel settings, string? smtpPasswordPlain = null) => Task.CompletedTask;
    public Task<string?> GetSmtpPasswordAsync() => Task.FromResult<string?>(null);
}
#endif
