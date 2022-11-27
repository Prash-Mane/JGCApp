using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JGC.DataBase
{
    public interface IRepository<T> where T : class, new()
    {
        Task<IList<T>> GetAsync();
        Task<List<T>> GetAsyncFromOldDB();
        Task<T> GetAsync(int id, bool withChilder = false);
        Task<IList<T>> GetAsync(Expression<Func<T, bool>> predicate, bool withChilder = false);
        Task RemoveAsync(Expression<Func<T, bool>> predicate);
        Task<int> InsertAsync(T entity);
        Task<int> InsertAllAsync(IEnumerable<T> range);
        Task<int> UpdateAsync(T entity);
        Task<int> UpdateAllAsync(IEnumerable<T> range);
        Task<int> DeleteAsync(T entity);
        Task DeleteAll();
        Task DeleteAll(IEnumerable<T> range);
        //Task DeleteAll(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<int> ExecuteAsync(string query);
        Task<T> ExecuteScalarAsync(string query);
        Task<Type> ExecuteScalarAsync<Type>(string query);
        Task<int> CountAsync();
        Task<IList<T>> QueryAsync(string query);
        Task<IList<Type>> QueryAsync<Type>(string query) where Type : new();
        Task InsertOrReplaceAsync(IEnumerable<T> range, bool withChilder = false);
        Task InsertOrReplaceAsync(T item);
        bool ChekIfOldDbExists();
        Task<bool> CopyAllOldDataAsync();
        // Task DeleteOldDb();
        //Task Transaction(Action action);
        SQLiteConnection GetConnection();
    }
}
