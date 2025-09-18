using MartaPol.Domain.Abstractions;
using MartaPol.Domain.Models;
using System.Text.RegularExpressions;
using MartaPol.Infrastructure.Data;

namespace MartaPol.Infrastructure.Services.Sheets;

public class SheetService : ISheetService
{
    private readonly MartaPolDb _db;
    private readonly IDateTimeProvider _clock;

    public SheetService(MartaPolDb db, IDateTimeProvider clock)
    { _db = db; _clock = clock; }

    public async Task SaveCurrentSessionAsSheetAsync(List<ScanRecord> records)
    {
        if (records.Count == 0) return;

        var date = _clock.NowLocal.Date;
        var baseName = date.ToString("yyyy-MM-dd");
        var existing = await _db.Conn.Table<Sheet>().Where(s => s.Name.StartsWith(baseName)).ToListAsync();
        string name = baseName;
        if (existing.Any(e => e.Name == baseName))
        {
            int n = 1;
            var rx = new Regex(@"^" + Regex.Escape(baseName) + @" paleta (\d+)$");
            foreach (var e in existing)
            {
                var m = rx.Match(e.Name);
                if (m.Success && int.TryParse(m.Groups[1].Value, out var i)) n = Math.Max(n, i);
            }
            name = $"{baseName} paleta {n + 1}";
        }

        var sheet = new Sheet { Id = Guid.NewGuid(), Name = name, CreatedAt = _clock.NowLocal };
        await _db.Conn.InsertAsync(sheet);
        foreach (var r in records) r.SheetId = sheet.Id;
        await _db.Conn.InsertAllAsync(records);
    }

    public Task<List<Sheet>> GetSheetsAsync(DateTime? from=null, DateTime? to=null, string? search=null)
    {
        var q = _db.Conn.Table<Sheet>();
        if (from.HasValue) q = q.Where(s => s.CreatedAt >= from.Value);
        if (to.HasValue) q = q.Where(s => s.CreatedAt < to.Value);
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(s => s.Name.Contains(search));
        return q.OrderByDescending(s => s.CreatedAt).ToListAsync();
    }

    public Task<List<ScanRecord>> GetSheetRecordsAsync(Guid sheetId)
        => _db.Conn.Table<ScanRecord>().Where(r => r.SheetId == sheetId).OrderBy(r => r.ScannedAt).ToListAsync();
}
