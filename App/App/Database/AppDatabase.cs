using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using App.Models;
using SQLite;

namespace App.Database
{
    public class AppDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<AppDatabase> Instance = new AsyncLazy<AppDatabase>(async () =>
        {
            var instance = new AppDatabase();
            var result = await Database.CreateTableAsync<User>();
            return instance;
        });

        public AppDatabase()
        {
            Database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        }
        
        public Task<User> GetUserAsync()
        {
            return Database.Table<User>().FirstOrDefaultAsync();
        }
        
        public Task<int> SaveUserAsync(User user)
        {
            Database.Table<User>().DeleteAsync();
            return Database.InsertAsync(user);
        }
    }
    
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        private readonly Lazy<Task<T>> _instance;

        public AsyncLazy(Func<T> factory)
        {
            _instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public AsyncLazy(Func<Task<T>> factory)
        {
            _instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return _instance.Value.GetAwaiter();
        }
    }
}