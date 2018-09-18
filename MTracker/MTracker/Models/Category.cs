using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;


namespace MTracker.Models
{
    public class Category : IEquatable<Category>
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name
        {
            get => _name; set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Color { get => _color; set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        #region Ignore

        private string _name;
        private string _color;

        public Category GetCopy()
        {
            return this.MemberwiseClone() as Category;
        }

        public void Notify()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("Color");
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

        public override bool Equals(object obj)
        {
            return Equals(obj as Category);
        }

        public bool Equals(Category other)
        {
            return other != null &&
                   ID == other.ID &&
                   Name == other.Name &&
                   Color == other.Color;
        }

        public override int GetHashCode()
        {
            var hashCode = 702216270;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Color);
            return hashCode;
        }
        #endregion


    }
}