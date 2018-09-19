using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using MTracker.Models;
using SQLite;
using System.Linq;

namespace MTracker.Data
{
    public class SyncedCategoryAccessor
    {

        public ObservableCollection<Category> ObservableList = new ObservableCollection<Category>();
        private SQLiteAsyncConnection database;

        private void openDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Entry>().Wait();
            database.CreateTableAsync<Category>().Wait();
        }

        public SQLiteAsyncConnection Database
        {
            get
            {
                if (database == null)
                    openDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Database.db3"));
                return database;
            }
        }


        public SyncedCategoryAccessor()
        {
            foreach (var item in Database.Table<Category>().ToListAsync().Result)
            {
                ObservableList.Add(item);
            }
            ObservableList.CollectionChanged += async (sender, e) => await Changed(sender, e);
        }

        public async Task Changed(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in args.NewItems)
                {
                    await AddAsync(item as Category);
                    return;
                }
            }
            if (args.Action == NotifyCollectionChangedAction.Move)
            {
                return;
            }
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in args.NewItems)
                {
                    await RemoveAsync(item as Category);
                    return;
                }
            }
            if (args.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (var item in args.NewItems)
                {
                    await AddAsync(item as Category);
                    return;
                }
            }
            return;
        }

        public async Task RemoveAsync(Category category)
        {
            await Database.DeleteAsync(category);
        }

        public async Task AddAsync(Category category)
        {
            if (category.ID != 0)
                await Database.UpdateAsync(category);
            else
                await Database.InsertAsync(category);
        }

        public void Add(Category category)
        {
            AddAsync(category).Wait();
        }

        public Category GetByID(int ID) => ObservableList.FirstOrDefault((obj) => obj.ID == ID+1);
    }
}
