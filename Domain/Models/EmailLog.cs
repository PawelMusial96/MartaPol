namespace MartaPol.Domain.Models;

public class EmailLog
{
    public Guid Id { get; set; }
    public DateTime SentAt { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Attachments { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? Error { get; set; }
}
