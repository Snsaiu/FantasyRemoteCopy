using FantasyRemoteCopy.Core.Extensions;

using SQLite;

namespace FantasyRemoteCopy.Core.Impls;

public abstract class DbBase
{
    protected readonly string dbPath;
    protected readonly SQLiteAsyncConnection connection;
    public DbBase()
    {
        dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "fantasyDb.db");
        connection = new SQLiteAsyncConnection(dbPath);
        CreateTableAsync().WaitTask(null, x => throw x);

    }

    /// <summary>
    /// 创建表
    /// </summary>
    protected abstract Task CreateTableAsync();

}