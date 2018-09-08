using System;
using SQLite;

namespace MTracker.Models
{
    public class Entry
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public int CategoryID { get; set; }
    }
}
