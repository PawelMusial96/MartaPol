#if ANDROID
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using MiniExcelLibs;

namespace MartaPol.Infrastructure.Services.Export;

public class XlsxExportService
{
    public async Task<string[]> ExportAsync(Guid sheetId)
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, $"sheet_{sheetId:N}.xlsx");
        var rows = new List<object> { new { Id = sheetId, Value = "Demo" } };
        await MiniExcel.SaveAsAsync(path, rows);
        return new[] { path };
    }
}
#else
using System;
using System.Threading.Tasks;

namespace MartaPol.Infrastructure.Services.Export;

public class XlsxExportService
{
    public Task<string[]> ExportAsync(Guid sheetId)
        => Task.FromResult(new[] { $"sheet_{sheetId:N}.xlsx" });
}
#endif
