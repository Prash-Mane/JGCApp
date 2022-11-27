using JGC.Common.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLiteNetExtensionsAsync.Extensions;
using SQLiteNetExtensions.Extensions;
using System.Linq;

namespace JGC.DataBase
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private static SQLiteAsyncConnection connection;

        private static SQLiteAsyncConnection oldconnection;

        public Repository()
        {
            try
            {
                ChangeConnection();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void ChangeConnection()
        {
            connection = DependencyService.Get<ISQLite>().GetAsyncConnection();
            oldconnection = DependencyService.Get<ISQLite>().GetAsyncOldDbConnection();
            CreateDatabaseAsync();
        }
        private void CreateDatabaseAsync()
        {
            if (connection == null)
            {
                return;
            }

            try
            {
                connection.CreateTableAsync<T>();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"{exception.Message} CreteDatabaseAsync; {typeof(T).GetType()}");
            }
        }

        public async Task<IList<T>> GetAsync()
        {
            if (connection == null)
            {
                return null;
            }

            try
            {
                return await connection.Table<T>().ToListAsync();
            }
            catch { return null; } //if table is being created
        }

        public async Task<List<T>> GetAsyncFromOldDB()
        {
            if (oldconnection == null)
            {
                return null;
            }

            try
            {
                return await oldconnection.Table<T>().ToListAsync();
            }
            catch { return null; }
        }

        public async Task<IList<T>> GetAsync(Expression<Func<T, bool>> predicate, bool withChildren = false)
        {
            if (connection == null)
            {
                return null;
            }

            if (predicate == null)
            {
                if (withChildren)
                    return await connection.GetAllWithChildrenAsync<T>();
                return await connection.Table<T>().ToListAsync();
            }

            if (withChildren)
                return await connection.GetAllWithChildrenAsync(predicate);
            return await connection.Table<T>().Where(predicate).ToListAsync();
        }

        public async Task RemoveAsync(Expression<Func<T, bool>> predicate)
        {
            if (connection == null)
                return;

            var itemsToDelete = await connection.Table<T>().Where(predicate).ToListAsync();
            await connection.DeleteAllAsync(itemsToDelete);
        }

        public async Task<T> GetAsync(int id, bool withChildren = false)
        {
            if (connection == null)
            {
                return null;
            }

            if (withChildren)
                return await connection.GetWithChildrenAsync<T>(id);

            var result = await connection.FindAsync<T>(id);
            return result;
        }

        public async Task<int> InsertAsync(T entity)
        {
            if (connection == null)
            {
                return default(int);
            }

            var result = await connection.InsertAsync(entity);
            return result;
        }

        public async Task<int> InsertAllAsync(IEnumerable<T> range)
        {
            if (connection == null || range == null || range.Count() == 0)
            {
                return default(int);
            }

            var result = await connection.InsertAllAsync(range);
            return result;
        }

        public async Task<int> UpdateAsync(T entity)
        {
            if (connection == null)
            {
                return default(int);
            }

            var result = await connection.UpdateAsync(entity);
            return result;
        }

        public async Task<int> UpdateAllAsync(IEnumerable<T> range)
        {
            if (connection == null)
            {
                return default(int);
            }

            var result = await connection.UpdateAllAsync(range);
            return result;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            if (connection == null)
            {
                return default(int);
            }

            var result = await connection.DeleteAsync(entity);

            return result;
        }

        public async Task DeleteAll()
        {
            if (connection == null)
            {
                return;
            }

            var tableName = typeof(T).GetTableName();

            await connection.ExecuteAsync($"Delete from {tableName}");

            //var result = await connection.DropTableAsync<T>();
            //await connection.CreateTableAsync<T>();
            //return result;
        }

        public async Task DeleteAll(IEnumerable<T> range)
        {
            if (connection == null)
            {
                return;
            }

            var tableName = typeof(T).GetTableName();

            await connection.DeleteAllAsync(range);

            //var result = await connection.DropTableAsync<T>();
            //await connection.CreateTableAsync<T>();
            //return result;
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            if (connection == null)
            {
                return null;
            }

            var result = await connection.FindAsync(predicate);
            return result;
        }

        public async Task<int> ExecuteAsync(string query)
        {
            if (connection == null)
            {
                return default(int);
            }

            var result = await connection.ExecuteAsync(query);

            return result;
        }

        public async Task<T> ExecuteScalarAsync(string query)
        {
            if (connection == null)
            {
                return null;
            }

            var result = await connection.ExecuteScalarAsync<T>(query);

            return result;
        }

        public async Task<Type> ExecuteScalarAsync<Type>(string query)
        {

            if (connection == null)
            {
                return default(Type);
            }

            var result = await connection.ExecuteScalarAsync<Type>(query);
            return result;
        }

        public async Task<int> CountAsync()
        {
            if (connection == null)
            {
                return default(int);
            }

            var result = await connection.Table<T>().CountAsync();
            return result;
        }

        public async Task<IList<T>> QueryAsync(string query)
        {
            if (connection == null)
            {
                return null;
            }

            var result = await connection.QueryAsync<T>(query);
            return result;
        }

        public async Task<IList<Type>> QueryAsync<Type>(string query) where Type : new()
        {
            if (connection == null)
            {
                return null;
            }

            var result = await connection.QueryAsync<Type>(query);
            return result;
        }

        public async Task InsertOrReplaceAsync(IEnumerable<T> range, bool withChildren = false)
        {
            if (connection == null || range == null)
                return;

            if (withChildren)
            {
                await connection.InsertOrReplaceAllWithChildrenAsync(range);
                return;
            }

            foreach (var item in range)
            {
                await connection.InsertOrReplaceAsync(item);
            }
        }

        public async Task InsertOrReplaceAsync(T item)
        {
            try
            {
                if (connection == null || item == null)
                    return;

                await connection.InsertOrReplaceAsync(item);
            }
            catch (Exception ex)
            {

            }

        }

        public bool ChekIfOldDbExists()
        {
            if (oldconnection != null)
                return true;
            else
                return false;

        }
        public async Task<bool> CopyAllOldDataAsync()
        {
            if (connection == null || oldconnection == null)
            {
                return false;
            }
            try
            {
                Debug.WriteLine($"{typeof(T).Name} trying to copy from old db");

                var mappingsOld = oldconnection.GetConnection().GetMapping<T>();
                var mappingsNew = connection.GetConnection().GetMapping<T>();

                //if (mappingsOld.HasAutoIncPK != mappingsNew.HasAutoIncPK
                //|| mappingsOld.Columns.Length != mappingsNew.Columns.Length
                //|| mappingsOld.MappedType != mappingsNew.MappedType
                //|| mappingsOld.PK.Name != mappingsNew.PK.Name
                //|| mappingsOld.TableName != mappingsNew.TableName)
                //return false;


                var columnsOld = oldconnection.GetConnection().Query<ColumnInfoExtended>($"PRAGMA table_info({mappingsOld.TableName})");
                var columnsNew = connection.GetConnection().Query<ColumnInfoExtended>($"PRAGMA table_info({mappingsNew.TableName})");

                if (columnsOld.Count != columnsNew.Count
                    || columnsOld.Count != mappingsNew.Columns.Length)
                {
                    Debug.WriteLine($"CopyAllOldDataAsync: {typeof(T).Name} skipped due to different columns number: {columnsOld.Count}/{columnsNew.Count}");
                    return false;
                }

                for (int i = 0; i < columnsOld.Count; i++)
                {
                    if (!columnsOld[i].Equals(columnsNew[i]))
                    {
                        Debug.WriteLine($"CopyAllOldDataAsync: {typeof(T).Name} skipped due to different columns signature: {columnsOld[i]}/{columnsNew[i]}");
                        return false;
                    }
                }

                //for (int i = 0; i < mappingsOld.Columns.Length; i++) 
                //{
                //    var colOld = mappingsOld.Columns[i];
                //    var colNew = mappingsNew.Columns[i];

                //    if (colOld.IsAutoGuid != colNew.IsAutoGuid
                //        || colOld.IsAutoInc != colNew.IsAutoInc
                //        || colOld.IsNullable != colNew.IsNullable
                //        || colOld.IsPK != colNew.IsPK
                //        || colOld.ColumnType != colNew.ColumnType
                //        || colOld.MaxStringLength != colNew.MaxStringLength
                //        || colOld.Name != colNew.Name
                //        || colOld.PropertyName != colNew.PropertyName)
                //        return false;
                //}

                var data = await oldconnection.Table<T>().ToListAsync();
                await DeleteAll();
                var result = await connection.InsertAllAsync(data);
                Debug.WriteLine($"{typeof(T).Name} copied successfully {data.Count} items");
                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"{exception.Message} CopyAllOldDataAsync; {typeof(T).GetType()}");
                return false;
            }
        }
        //public async Task DeleteOldDb()
        //{
        //    DependencyService.Get<ISQLite>().DeleteOldDb();
        //}

        //public async Task Transaction(Action action) 
        //{
        //    if (connection == null)
        //        return;

        //    await connection.RunInTransactionAsync((SQLiteConnection conn) => action.Invoke());
        //}

        //public async Task DeleteAll(Expression<Func<T, bool>> predicate)
        //{
        //    if (connection == null)
        //    {
        //        return;
        //    }

        //    var itemsToDelete = await GetAsync(predicate);
        //    await connection.DeleteAllAsync(itemsToDelete);
        //}


        public SQLiteConnection GetConnection()
        {
            return DependencyService.Get<ISQLite>().GetConnection(); ;

        }
        class ColumnInfoExtended
        {
            public string name { get; set; }
            public string @type { get; set; }
            public byte notnull { get; set; }
            public string dflt_value { get; set; }
            public byte pk { get; set; }

            public override bool Equals(object obj)
            {
                var otherObj = obj as ColumnInfoExtended;
                return ToString() == otherObj.ToString();
            }

            public override string ToString()
            {
                return $"{name}{@type}{notnull}{dflt_value}{pk}";
            }
        }
    }
}
