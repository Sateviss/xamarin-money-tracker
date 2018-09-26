using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MTracker.Models;
using SQLite;

namespace MTracker.Data
{
    public class DynamicEntryAccessor : IEnumerable<Entry>, INotifyCollectionChanged
    {
        protected const int rollover = 64;
        protected int limit = rollover;
        public List<Entry> bottomList;
        private bool skipA;

        private Entry _bottomEntry;

        public Entry BottomEntry
        {
            get
            {
                return _bottomEntry;
            }
            private set
            {
                _bottomEntry = value;
                MaxedOut = _bottomEntry.Equals(bottomestEntry) ? true : false;
            }
        }

        protected int bottomEntryIndex = -1;
        private Entry bottomestEntry;

        public bool MaxedOut;

        public void UpLimit(int value)
        {
            limit += value;
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                Database.QueryAsync<Entry>(
                                           "SELECT * " +
                                           "FROM [Entry] " +
                                           "ORDER BY [Date] DESC , [ID] ASC " +
                                           $"LIMIT {limit - value},{value}").Result));

        }

        public class EntryEnum : IEnumerator<Entry>
        {

            private DynamicEntryAccessor parent;
            private int index = -1;

            public Entry lastBuffered = null;
            protected int offset = 0;
            protected List<Entry> bufferList = new List<Entry>();

            public void RefreshBuffer()
            {
                bufferList = parent.Database.QueryAsync<Entry>(
                                                               "SELECT * " +
                                                               "FROM [Entry] " +
                                                               "ORDER BY [Date] DESC , [ID] ASC " +
                                                               $"LIMIT {offset},{(offset > parent.limit ? 0 : Math.Min(rollover, parent.limit - offset))}").Result;
                if (bufferList.Count != 0)
                    lastBuffered = bufferList.Count >= 1 ? bufferList.Last() : lastBuffered;
                if (parent.bottomEntryIndex < index + bufferList.Count || (parent.BottomEntry == null && lastBuffered != null))
                {
                    parent.bottomList = bufferList;
                    parent.BottomEntry = lastBuffered;
                    parent.bottomEntryIndex = index;
                }
            }

            public EntryEnum(DynamicEntryAccessor papa)
            {
                parent = papa;
                RefreshBuffer();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public Entry Current => bufferList[index];
            private object Current1 => this.Current;
            object IEnumerator.Current => Current1;

            private bool disposedValue = false;
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposedValue)
                {
                    if (disposing)
                    {
                        bufferList = null;
                    }
                    lastBuffered = null;
                }
                this.disposedValue = true;
            }

            public bool MoveNext()
            {
                index++;
                if (index == rollover)
                {
                    index = 0;
                    offset += rollover;
                    RefreshBuffer();
                    System.Diagnostics.Debug.WriteLine($"Paginated {bufferList.Count}");
                }

                if (parent.SkipA && bufferList.Count != 0 && bufferList[index >= bufferList.Count ? bufferList.Count - 1 : index].Name.Contains("а"))
                    return MoveNext();

                return index < bufferList.Count;
            }
        }

        private SQLiteAsyncConnection database;

        public DynamicEntryAccessor()
        {
            var res = Database.QueryAsync<Entry>(
                                                 "SELECT * " +
                                                 "FROM [Entry] " +
                                                 "ORDER BY [Date] ASC , [ID] DESC " +
                                                 $"LIMIT 1").Result;
            bottomestEntry = res.Count == 0 ? null : res[0];
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

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

        public bool SkipA { get => skipA; set { skipA = value; CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, null)); } }

        public IEnumerator<Entry> GetEnumerator()
        {
            return new EntryEnum(this);
        }

        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        public async Task RemoveAsync(Entry entry)
        {
            await Database.DeleteAsync(entry);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, entry));
        }

        public async Task AddAsync(Entry entry)
        {
            if (entry.ID != 0)
                await Database.UpdateAsync(entry);
            else
                await Database.InsertAsync(entry);

            var res = await Database.QueryAsync<Entry>(
                                                 "SELECT * " +
                                                 "FROM [Entry] " +
                                                 "ORDER BY [Date] ASC , [ID] DESC " +
                                                 $"LIMIT 1");
            bottomestEntry = res.Count == 0 ? null : res[0];

            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, entry));
        }

        public void Add(Entry entry)
        {
            AddAsync(entry).Wait();
        }

        public int CountByCategoryID(int ID)
        {
            return Task.Run(() => CountByCategoryIDAsync(ID)).GetAwaiter().GetResult();
        }

        public async Task<int> CountByCategoryIDAsync(int ID)
        {
            var queryRes =
                await Database.QueryAsync<Entry>(
                 "SELECT * " +
                 "FROM [Entry] " +
                $"WHERE [CategoryID] = {ID - 1}");
            return queryRes.Count();
        }

        public List<Entry> GetFromDate(DateTime date)
        {
            return Task.Run(() => GetFromDateAsync(date)).GetAwaiter().GetResult();
        }

        public async Task<List<Entry>> GetFromDateAsync(DateTime date)
        {
            return await Database.QueryAsync<Entry>(
                         "SELECT * " +
                         "FROM [Entry] " +
                        $"WHERE [Date] > {date.ToShortDateString()}");
        }
    }
}
