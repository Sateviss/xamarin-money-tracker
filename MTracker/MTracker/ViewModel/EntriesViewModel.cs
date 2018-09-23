using System.Collections.Generic;
using System.Threading.Tasks;
using MTracker.Data;
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

        public bool EditVisible => SelectedEntries.Count == 1;
        public ToolbarItem EditItem;

        const int rotationTime = 250;

        public EntriesViewModel(ContentPage page)
        {
            contentPage = page;
            Title = "Entries";

            SelectedEntries = new List<Models.Entry>();
            Entries = App.EntryAccessor;

            DeleteItem = new ToolbarItem("", "delete_icon.xml", async () => { await DeleteSelected(); });
            EditItem = new ToolbarItem("", "edit_icon.xml", async () => { await EditSelected(); });

        }

        public void LoadMore(Models.Entry entry)
        {
            if (entry == Entries.BottomEntry && !Entries.MaxedOut)
            {
                Entries.UpLimit(64);
                OnPropertyChanged("Entries");
            }
        }

        public Command ClickCommand => new Command(async (o) => { await ClickEntry(o); });

        public async Task ClickEntry(object o)
        {
            var grid = o as Grid;
            var entry = grid.BindingContext as Models.Entry;
            var rotationTask = grid.RotateYTo(180 - entry.Rotation, rotationTime);
            await Task.Delay(rotationTime / 2);
            entry.Selected = !entry.Selected;

            if (SelectedEntries.Contains(entry))
                SelectedEntries.Remove(entry);
            else
                SelectedEntries.Add(entry);
            CheckToolbar();

            await rotationTask;
        }

        private void CheckToolbar()
        {
            if (DeleteVisible)
            {
                if (!contentPage.ToolbarItems.Contains(DeleteItem))
                    ToolbarItems.Add(DeleteItem);
            }
            else
                ToolbarItems.Remove(DeleteItem);

            if (EditVisible)
            {
                if (!contentPage.ToolbarItems.Contains(EditItem))
                    ToolbarItems.Add(EditItem);
            }
            else
                ToolbarItems.Remove(EditItem);
                
            OnPropertyChanged("ToolbarItems");
        }

        private async Task DeleteSelected()
        {
            var dialog = await contentPage.DisplayAlert(
                "",
                $"Are you sure you want to delete {(SelectedEntries.Count==1?"this entry":$"these {SelectedEntries.Count} entries")}?",
                "Yes",
                "No");
            if (dialog == false)
                return;
            SelectedEntries.ForEach((obj) => obj.Selected = false);
            SelectedEntries.ForEach(async (obj) => await Entries.RemoveAsync(obj));
            SelectedEntries.Clear();
            OnPropertyChanged("Entries");
            CheckToolbar();
        }

        private async Task EditSelected()
        {
            var newItem = await NewEntryViewModel.EditEntry(contentPage, SelectedEntries[0]);
            if (newItem != null)
            {
                await App.EntryAccessor.AddAsync(newItem);
            }
        }
    }
}
