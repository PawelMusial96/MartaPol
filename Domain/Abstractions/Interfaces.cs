namespace MartaPol.Domain.Abstractions;

public interface IDateTimeProvider { DateTime NowLocal { get; } DateTime UtcNow { get; } }
public interface IDeviceFeedback { void Vibrate(int ms); void Beep(); }
public interface ISheetService { Task SaveCurrentSessionAsSheetAsync(List<MartaPol.Domain.Models.ScanRecord> records); Task<List<MartaPol.Domain.Models.Sheet>> GetSheetsAsync(DateTime? from=null, DateTime? to=null, string? search=null); Task<List<MartaPol.Domain.Models.ScanRecord>> GetSheetRecordsAsync(Guid sheetId); }
public interface IExportService { Task<string[]> ExportAsync(Guid sheetId, MartaPol.Domain.Enums.ExportFormat format); }
public interface IEmailService { Task<bool> SendAsync(IEnumerable<string> attachmentPaths, string subject, string to, CancellationToken ct = default); }
public interface IDialogService { Task<bool> ConfirmAsync(string title, string message); Task<string?> PromptAsync(string title, string message); Task ToastAsync(string message); }
public interface ISettingsService {
    Task EnsureLoadedAsync();
    Task<IEnumerable<MartaPol.Domain.Models.EanVariableWeightRule>> GetEanRulesAsync();
    Task<MartaPol.Domain.Enums.DuplicatePolicy> GetDuplicatePolicyAsync();
    Task<(bool sound,bool vib)> GetFeedbackAsync();
    Task<int> GetSheetRecordLimitAsync();
    Task<MartaPol.Domain.Enums.ExportFormat> GetExportFormatAsync();
    Task<MartaPol.Domain.Models.SettingsModel> GetAllAsync();
    Task SaveAsync(MartaPol.Domain.Models.SettingsModel settings, string? smtpPasswordPlain = null);
    Task<string?> GetSmtpPasswordAsync();
}

public interface ISoundService { void Beep(); }
