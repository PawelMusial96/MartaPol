#if ANDROID
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace MartaPol.Infrastructure.Services.Export;

public class CsvExportService
{
    public async Task<string[]> ExportAsync(Guid sheetId)
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, $"sheet_{sheetId:N}.csv");
        var content = "Id,Value\n" + $"{sheetId},Demo\n";
        await File.WriteAllTextAsync(path, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        return new[] { path };
    }
}
#else
using System;
using System.Threading.Tasks;

namespace MartaPol.Infrastructure.Services.Export;

public class CsvExportService
{
    public Task<string[]> ExportAsync(Guid sheetId)
        => Task.FromResult(new[] { $"sheet_{sheetId:N}.csv" });
}
#endif
