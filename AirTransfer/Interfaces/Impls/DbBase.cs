using AirTransfer.Extensions;
using SQLite;

namespace AirTransfer.Interfaces.Impls;

public abstract class DbBase
{
    protected readonly string dbPath;
    protected readonly SQLiteAsyncConnection connection;

    public DbBase()
    {
        dbPath = Path.Combine(FileSystem.AppDataDirectory, "fantasyDb.db");
        if (!Directory.Exists(FileSystem.AppDataDirectory))
            Directory.CreateDirectory(FileSystem.AppDataDirectory);

        connection = new SQLiteAsyncConnection(dbPath);

        // ReSharper disable once VirtualMemberCallInConstructor
        CreateTableAsync().WaitTask(null, x => throw x);
    }

    /// <summary>
    /// 创建表
    /// </summary>
    protected abstract Task CreateTableAsync();
}