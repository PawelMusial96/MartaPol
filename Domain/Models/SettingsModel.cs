using MartaPol.Domain.Enums;

namespace MartaPol.Domain.Models;

public class SettingsModel
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SmtpEncryption { get; set; } = "STARTTLS";
    public string SmtpUser { get; set; } = string.Empty;
    public string SmtpPasswordKey { get; set; } = "smtp_pwd";
    public string Recipient { get; set; } = string.Empty;

    public DuplicatePolicy DuplicatePolicy { get; set; } = DuplicatePolicy.Ask;
    public bool Beep { get; set; } = true;
    public bool Vibrate { get; set; } = true;
    public int SheetRecordLimit { get; set; } = 1000;
    public ExportFormat ExportFormat { get; set; } = ExportFormat.Csv;

    public string Theme { get; set; } = "System";
}
