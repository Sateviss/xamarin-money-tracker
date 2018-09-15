using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace MTracker.Models
{
    public class Entry : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } = 0;

        public DateTime Date { get; set; } = DateTime.Now;

        public string Name { get; set; }
        public float Amount { get; set; } = 0f;
        public string Currency { get; set; }
        public int CategoryID { get; set; } = -1;

        #region Ignore
        public float Rotation => Selected ? 360 : 180;
        public string Icon => Selected ? "tick_icon.xml" : null;

        public bool Selected { get => selected; 
            set
            {
                selected = value;
                OnPropertyChanged("Rotation");
                OnPropertyChanged("Selected");
                OnPropertyChanged("Icon");
            }
        }

        private bool selected = false;

        public override bool Equals(object obj)
        {
            return obj is Entry entry &&
                   ID == entry.ID &&
                   Date == entry.Date &&
                   Name == entry.Name &&
                   Amount == entry.Amount &&
                   Currency == entry.Currency &&
                   CategoryID == entry.CategoryID;
        }

        public override int GetHashCode()
        {
            var hashCode = -1561217998;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + Date.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Amount.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Currency);
            hashCode = hashCode * -1521134295 + CategoryID.GetHashCode();
            return hashCode;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #endregion

    }
}
