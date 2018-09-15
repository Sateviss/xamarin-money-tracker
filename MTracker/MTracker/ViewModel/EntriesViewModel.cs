using System;
using System.Collections.Generic;
using MTracker.Models;
using MTracker.Views;
using MTracker.Data;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MTracker.ViewModel
{
    public class EntriesViewModel : BaseViewModel
    {
        private readonly ContentPage contentPage;

        public DynamicEntryAccessor Entries { get; private set; }
        public List<Models.Entry> SelectedEntries { get; private set; }

        public List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();

        public bool DeleteVisible => SelectedEntries.Count != 0;
        public ToolbarItem DeleteItem;

        const int rotationTime = 250;

        public EntriesViewModel(ContentPage page)
        {
            contentPage = page;
            Title = "Entries";

            SelectedEntries = new List<Models.Entry>();
            Entries = App.EntryAccessor;

            DeleteItem = new ToolbarItem("", "delete_icon.xml", DeleteSelected);

        }

        public void LoadMore(Models.Entry entry)
        {
            if (entry == Entries.BottomEntry && !Entries.MaxedOut)
            {
                Entries.UpLimit(64);
                OnPropertyChanged("Entries");
            }
        }

        public Command ClickCommand => new Command(ClickEntry);

        public void ClickEntry(object o)
        {
            var grid = o as Grid;
            var entry = grid.BindingContext as Models.Entry;

            grid.RotateYTo(180-entry.Rotation, rotationTime);

            entry.Selected = !entry.Selected;
            if (SelectedEntries.Contains(entry))
                SelectedEntries.Remove(entry);
            else
                SelectedEntries.Add(entry);

            CheckDeleteToolbar();

            System.Diagnostics.Debug.WriteLine("".PadLeft(20, '#'));
            foreach (var item in SelectedEntries)
            {
                System.Diagnostics.Debug.WriteLine(item.ID);
            }
        }

        private void CheckDeleteToolbar()
        {
            if (DeleteVisible)
            {
                if (contentPage.ToolbarItems.Contains(DeleteItem))
                    return;
                ToolbarItems.Add(DeleteItem);
            }
            else
                ToolbarItems.Remove(DeleteItem);
            OnPropertyChanged("ToolbarItems");
        }

        private void DeleteSelected()
        {
            SelectedEntries.ForEach((obj) => obj.Selected = false);
            SelectedEntries.ForEach(async (obj) => await Entries.RemoveAsync(obj));
            SelectedEntries.Clear();
            OnPropertyChanged("Entries");
            CheckDeleteToolbar();
        }
    }
}
