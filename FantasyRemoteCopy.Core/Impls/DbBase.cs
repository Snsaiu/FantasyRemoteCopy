using SQLite;

namespace FantasyRemoteCopy.Core.Impls;

public abstract class DbBase
{
    protected readonly string dbPath;
    protected SQLiteConnection connection;
    public DbBase()
    {
        this.dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "fantasyDb.db");

        this.connection = new SQLiteConnection(this.dbPath);

        if (this.ExistTable()==false)
        {
            this.CreateTable();
        }
    }

    /// <summary>
    /// 创建表
    /// </summary>
    protected abstract void CreateTable();

    /// <summary>
    /// 检查表是否已经存在
    /// </summary>
    /// <returns>存在返回true，否则返回false</returns>
    protected abstract bool ExistTable();
}