using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using MTracker.Models;
using MTracker.Views;
using Xamarin.Forms;

namespace MTracker.ViewModel
{
    public class NewEntryViewModel : BaseViewModel
    {
        public Models.Entry Entry;

        public TaskCompletionSource<Models.Entry> taskCompletion;
        public ObservableCollection<Category> Categories = App.CategoryAccessor.ObservableList;

        private TimeSpan time;
        public TimeSpan Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                Entry.Date = Entry.Date + time;
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                Entry.Date = date + Entry.Date.TimeOfDay;
            }
        }

        public string EntryTitle
        {
            get => Entry.Name;
            set => Entry.Name = value;
        }

        public int CategoryID
        {
            get => Entry.CategoryID;
            set => Entry.CategoryID = value;
        }

        public string SelectedCategory => CategoryID == -1 ? "" : App.CategoryAccessor.GetByID(CategoryID).Name;

        public string Amount
        {
            get => Math.Abs(Entry.Amount - Models.Entry.ValueNull) < 0.0000001f ? null : Entry.Amount.ToString();
            set
            {
                var ok = float.TryParse(value, out float res);
                Entry.Amount = ok ? res : Models.Entry.ValueNull;
            }
        }

        public event Action OnCategoryError;
        public event Action OnTitleError;
        public event Action OnAmountError;

        public void Accept()
        {
            bool allClear = true;
            if (CategoryID == -1)
            {
                allClear = false;
                OnCategoryError();
            }
            if (string.IsNullOrEmpty(EntryTitle))
            {
                allClear = false;
                OnTitleError();
            }
            if (Amount == null || Entry.Amount > 999e21)
            {
                allClear = false;
                OnAmountError();
            }
            if (allClear)
                taskCompletion.SetResult(Entry);
        }

        public void Cancel()
        {
            taskCompletion.SetResult(null);
        }


        public NewEntryViewModel(Models.Entry myEntry)
        {
            Title = myEntry.ID == 0?"New entry":"Editing entry";
            Entry = myEntry;
            date = Entry.Date;
            time = Entry.Date.TimeOfDay;
            taskCompletion = new TaskCompletionSource<Models.Entry>();
        }

        public static async Task<Models.Entry> EditEntry(Page currentPage, Models.Entry entry)
        {
            var page = new NewEntryPage(entry);
            await currentPage.Navigation.PushModalAsync(new NavigationPage(page));
            await ((NewEntryViewModel)page.BindingContext).taskCompletion.Task;
            Models.Entry result = ((NewEntryViewModel)page.BindingContext).taskCompletion.Task.Result;
            await currentPage.Navigation.PopModalAsync();
            return result;
        }
    }
}
