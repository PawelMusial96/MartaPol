namespace MartaPol.Domain.Models;

public class ScanRecord
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public DateTime ScannedAt { get; set; }
    public Guid? SheetId { get; set; }
}
