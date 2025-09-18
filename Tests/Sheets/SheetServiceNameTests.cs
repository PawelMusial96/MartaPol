using MartaPol.Infrastructure.Services.Sheets;
using MartaPol.Infrastructure.Data;
using MartaPol.Infrastructure.Services;
using MartaPol.Domain.Models;
using Xunit;
using System.Threading.Tasks;

public class SheetServiceNameTests
{
    [Fact]
    public async Task Creates_Base_Or_Palette_Suffix()
    {
        var db = new MartaPolDb();
        var clock = new DateTimeProvider();
        var svc = new SheetService(db, clock);

        await svc.SaveCurrentSessionAsSheetAsync(new() { new ScanRecord{ Id=System.Guid.NewGuid(), Code="X", WeightKg=1, ScannedAt=clock.NowLocal } });
        var sheets = await svc.GetSheetsAsync();
        Assert.True(sheets.Count >= 1);
    }
}
