using System;
using System.Collections.Generic;
using System.IO;
using MTracker.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MTracker.Data
{
    public class EntryAccessor
    {
        static Database database;

        private static Database Database
        {
            get
            {
                if (database == null)
                    database = new Database(Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Database.db3"));
                return database;
            }
        }

        public async Task<List<Entry>> GetEntries(int count = 0, int offset=0, long timeOffset = long.MaxValue, bool reverseChronological=true)
        {
            var list = await Database.GetEntriesAsync();

            list = list.FindAll((obj) => obj.Date > DateTime.Now.AddSeconds(-timeOffset));
            list = list.GetRange(offset, count == 0? -1:count);
            if (reverseChronological)
                list.Reverse();
            return list;
        }
        public async void SaveEntry(Entry entry) => await Database.SaveEntryAsync(entry);
        public async void RemoveEntry(Entry entry) => await Database.DeleteEntryAsync(entry);


    }
}
