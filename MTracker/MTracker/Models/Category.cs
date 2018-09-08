using System;
using SQLite;


namespace MTracker.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Color { get; set; }
    }
}