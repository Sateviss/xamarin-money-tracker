using System.Collections.Generic;
using System.Threading.Tasks;
using MTracker.Models;
using SQLite;

namespace MTracker.Data
{
    public class Database
    {
        readonly SQLiteAsyncConnection database;

        public Database(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Entry>().Wait();
            database.CreateTableAsync<Category>().Wait();
        }

        public Task<List<Entry>> GetEntriesAsync() => database.Table<Entry>().ToListAsync();
        public Task<Entry> GetEntryAsync(int id) => database.Table<Entry>().Where(i => i.ID == id).FirstOrDefaultAsync();
        public Task<int> SaveEntryAsync(Entry item) => (item.ID != 0) ? database.UpdateAsync(item) : database.InsertAsync(item);
        public Task<int> DeleteEntryAsync(Entry item) => database.DeleteAsync(item);

        public Task<List<Category>> GetCategoriesAsync() => database.Table<Category>().ToListAsync();
        public Task<Category> GetCategoryAsync(int id) => database.Table<Category>().Where(i => i.ID== id).FirstOrDefaultAsync();
        public Task<int> SaveCategoryAsync(Category item) => (item.ID != 0) ? database.UpdateAsync(item) : database.InsertAsync(item);
        public Task<int> DeleteCategoryAsync(Category item) => database.DeleteAsync(item);

    }
}
