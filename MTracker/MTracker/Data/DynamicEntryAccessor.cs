using System;
using System.Collections.Generic;
using System.IO;
using MTracker.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using SQLite;

namespace MTracker.Data
{
    public class DynamicEntryAccessor : IEnumerable<Entry>
    {
        public class EntryEnum : IEnumerator<Entry>
        {
            private const int rollover = 10;

            private int offset = 0;
            private int index = -1;

            private List<Entry> bufferList;
            private SQLiteAsyncConnection database;

            public EntryEnum(SQLiteAsyncConnection connection)
            {
                database = connection;
                bufferList = database.QueryAsync<Entry>("SELECT * " +
                                                        "FROM [Entry] " +
                                                        "ORDER BY [Date] DESC " +
                                                        $"LIMIT {offset},{rollover}").Result;
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
                    bufferList = null;
                    database = null;
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

                    bufferList = database.QueryAsync<Entry>("SELECT * " +
                                                            "FROM [Entry] " +
                                                            "ORDER BY [Date] DESC " +
                                                            $"LIMIT {offset},{rollover}").Result;
                }
                return index < bufferList.Count;
            }
        }

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


        public IEnumerator<Entry> GetEnumerator()
        {
            return new EntryEnum(Database);
        }

        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }
    }
}
