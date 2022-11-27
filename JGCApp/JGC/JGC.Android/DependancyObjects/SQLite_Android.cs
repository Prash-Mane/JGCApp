using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using System.Diagnostics;
using JGC.Common.Constants;
using JGC.Common.Interfaces;
using JGC.Droid.DependancyObjects;

[assembly: Dependency(typeof(SQLite_Android))]
namespace JGC.Droid.DependancyObjects
{
    public class SQLite_Android : ISQLite
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var sqliteFilename = AppSetting.APP_DBNAME;
            var path = Path.Combine(documentsPath, sqliteFilename);

            if (!File.Exists(path))
            {
                File.Create(path);
            }

            var conn = new SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            return conn;
        }

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = AppSetting.APP_DBNAME;
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            Console.WriteLine(path);
            if (!File.Exists(path)) File.Create(path);
            var conn = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            return conn;
        }

        public SQLiteAsyncConnection GetAsyncOldDbConnection()
        {
            string lastUsedDbPath = null;
            DateTime lastDbReadTime = default(DateTime);
            foreach (var dbFilePaths in Directory.EnumerateFiles(documentsPath, "*.db3"))
            {
                if (Path.GetFileName(dbFilePaths) != AppSetting.APP_DBNAME)
                {
                    var accessTime = File.GetLastAccessTime(dbFilePaths);
                    if (accessTime > lastDbReadTime)
                    {
                        lastDbReadTime = accessTime;
                        lastUsedDbPath = dbFilePaths;
                    }
                }

            }
            if (lastUsedDbPath != null)
                return new SQLiteAsyncConnection(lastUsedDbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            else
                return null;
        }
        public void DeleteOldDb()
        {
            foreach (var dbFilePath in Directory.EnumerateFiles(documentsPath, "*.db3"))
            {
                if (Path.GetFileName(dbFilePath) == AppSetting.APP_DBNAME)
                    continue;
                try
                {
                    File.Delete(dbFilePath);
                }
                catch
                {
                    Debug.WriteLine($"{dbFilePath} was not removed");
                }
            }
        }


        public SQLite_Android()
        {

        }
    }
}