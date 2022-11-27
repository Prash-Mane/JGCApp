using SQLite;

namespace JGC.Common.Interfaces
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection(); 
        SQLiteAsyncConnection GetAsyncConnection();

        //SQLiteConnection GetConnectionNew();
       SQLiteAsyncConnection GetAsyncOldDbConnection();
        //void DeleteOldDb();
    }
}
