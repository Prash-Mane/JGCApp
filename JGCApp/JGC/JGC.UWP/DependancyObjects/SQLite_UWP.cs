using JGC.Common.Constants;
using JGC.Common.Interfaces;
using JGC.DataBase.DataTables;
using JGC.UWP.DependancyObjects;
using SQLite;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_UWP))]
namespace JGC.UWP.DependancyObjects
{
   public class SQLite_UWP : ISQLite
    {
        public const string AppDBPath = @"JGC-DB-20190501.sqlite";
        public const string PackageDBPath = @"Assets\JGC-DB-20190501.sqlite";

        string documentsPath = ApplicationData.Current.LocalFolder.Path;
       //string dd = await Package.Current.InstalledLocation.GetFileAsync(PackageDBPath);

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            //var sqliteFilename = AppSetting.APP_DBNAME;
            //var path = Path.Combine(documentsPath, sqliteFilename);

            //if (!File.Exists(path))
            //{
            //    File.Create(path);
            //}

            //var conn = new SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            //return conn;
            string documentPath = ApplicationData.Current.LocalFolder.Path;
            string path = Path.Combine(documentPath, AppSetting.APP_DBNAME);

            return new SQLiteAsyncConnection(path);
        }

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = AppSetting.APP_DBNAME;
            string documentsPath = ApplicationData.Current.LocalFolder.Path;
            var path = Path.Combine(documentsPath, sqliteFilename);
            if (!File.Exists(path)) File.Create(path);
            var conn = new SQLiteConnection(path);
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

        public SQLite_UWP()
        {
                
        }       
    }
}
