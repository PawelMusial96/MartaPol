#if ANDROID
using System;
using System.IO;
using Microsoft.Maui.Storage;
using SQLite;

namespace MartaPol.Infrastructure.Data;

public class MartaPolDb
{
    private readonly Lazy<SQLiteAsyncConnection> _conn;

    public MartaPolDb()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "martapol.db3");
        _conn = new(() => new SQLiteAsyncConnection(dbPath,
            SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));
    }

    public SQLiteAsyncConnection Connection => _conn.Value;

    // Alias wymagany przez SheetService
    public SQLiteAsyncConnection Conn => _conn.Value;
}
#else
using System;
using SQLite;

namespace MartaPol.Infrastructure.Data;

public class MartaPolDb
{
    // Wariant net8.0 – tylko do kompilacji testów (nieu¿ywany w runtime)
    public SQLiteAsyncConnection Connection => throw new NotSupportedException();
    public SQLiteAsyncConnection Conn => throw new NotSupportedException();
}
#endif
