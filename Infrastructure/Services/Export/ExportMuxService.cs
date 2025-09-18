using MartaPol.Domain.Abstractions;
using MartaPol.Domain.Enums;

namespace MartaPol.Infrastructure.Services.Export;

public class ExportMuxService : IExportService
{
    private readonly CsvExportService _csv;
    private readonly XlsxExportService _xlsx;

    public ExportMuxService(CsvExportService csv, XlsxExportService xlsx)
    { _csv = csv; _xlsx = xlsx; }

    public async Task<string[]> ExportAsync(Guid sheetId, ExportFormat format)
    {
        if (format == ExportFormat.Csv) return await _csv.ExportAsync(sheetId);
        return await _xlsx.ExportAsync(sheetId);
    }
}
