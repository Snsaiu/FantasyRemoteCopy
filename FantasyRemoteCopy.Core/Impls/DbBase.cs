using SQLite;

namespace FantasyRemoteCopy.Core.Impls;

public abstract class DbBase
{
    protected readonly string dbPath;
    protected SQLiteConnection connection;
    public DbBase()
    {
        this.dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "fantasyDb.db");

        this.connection = new SQLiteConnection(this.dbPath);
        this.CreateTable();
        
    }

    /// <summary>
    /// 创建表
    /// </summary>
    protected abstract void CreateTable();

}