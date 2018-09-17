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
        public float Amount { get; set; } = ValueNull;
        public string Currency { get; set; }
        public int CategoryID { get; set; } = -1;

        #region Ignore

        public const float ValueNull = -0.80085135f;
        public const int CutLength = 16;

        public string AmountString 
        {
            get
            {
                int log1000 = (int)Math.Floor(Math.Log10(Amount)/3);
                var powers = new List<string> {"", "K", "M", "G", "T", "P", "E", "Z" };
                var ret = log1000>0?
                    string.Format("{0:F0}{1} BYN", Amount / (Math.Pow(10, 3*log1000)), powers[log1000]) :
                    string.Format("{0:F2} BYN", Amount);
                return ret;
            }
        }

        public string NameShort 
        {
            get
            {
                return Name.Length <= CutLength ? Name : Name.Substring(0, CutLength) + "…";
            }
        }

        public float Rotation => Selected ? 360 : 180;
        public float IconOpacity => Selected?1:0;

        public bool Selected 
        { get => selected; 
            set
            {
                selected = value;
                OnPropertyChanged("Rotation");
                OnPropertyChanged("Selected");
                OnPropertyChanged("IconOpacity");
            }
        }

        private bool selected = false;

        public override bool Equals(object obj)
        {
            return obj is Entry entry &&
                   ID == entry.ID &&
                   Date == entry.Date &&
                   Name == entry.Name &&
                   Math.Abs(Amount - entry.Amount) < 0.00001f &&
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
